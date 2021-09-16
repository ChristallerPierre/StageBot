using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using StageBot.Services;
using System;
using System.Threading.Tasks;

namespace StageBot.Setup
{
	public class BotStartup : IBotStartup
	{
		private string _botToken;
		private DiscordSocketClient _client;

		public BotStartup(IOptions<Secrets> secrets)
		{
			_botToken = secrets.Value.BotToken;
		}

		public async Task MainAsync()
		{
			//try {
				_client = new DiscordSocketClient();
				_client.Log += LoggingService.Log;
				_client.Connected += StartCommandHandler;
				_client.Disconnected += StopCommandHandler;
				_client.Ready += ClientReady;

				await _client.LoginAsync(TokenType.Bot, _botToken);
				await _client.StartAsync();

				// block this task until program is closed
				await Task.Delay(-1);
			//} catch (Exception e) {
			//	await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(MainAsync), "Error", e));
			//}
		}

		public async Task StartCommandHandler()
		{
			var handler = new CommandHandler(_client, new CommandService());
			await handler.InstallCommandsAsync();
		}

		public async Task StopCommandHandler(Exception exception)
		{
			var message = new LogMessage(LogSeverity.Error, nameof(StopCommandHandler), "Bot disconnected", exception);
			await LoggingService.Log(message);
		}

		public async Task ClientReady()
		{
			// do something here, probably
			var message = new LogMessage(LogSeverity.Info, nameof(ClientReady), "Client is ready");
			await LoggingService.Log(message);
		}
	}
}
