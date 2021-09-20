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
		private CommandHandler _handler;
		private IServiceProvider _serviceProvider;

		public BotStartup(IOptions<Secrets> secrets, CommandHandler handler, IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_botToken = secrets.Value.BotToken;
			_handler = handler;
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
			_handler = new CommandHandler(_serviceProvider, _client, new CommandService());
			await _handler.InitializeAsync();
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
