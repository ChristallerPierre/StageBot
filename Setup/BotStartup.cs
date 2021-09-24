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
		private IServiceProvider _serviceProvider;

		public BotStartup(IOptions<Secrets> secrets, IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
			_botToken = secrets.Value.BotToken;
		}

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_client.Log += LoggingService.Log;
			_client.Connected += StartCommandHandler;
			_client.Disconnected += StopCommandHandler;
			_client.Ready += ClientReady;

			await _client.LoginAsync(TokenType.Bot, _botToken);
			await _client.StartAsync();

			// block this task until program is closed
			await Task.Delay(-1);
		}

		// todo : have the bot say hello when starting

		public async Task StartCommandHandler()
		{
			var serviceConfig = new CommandServiceConfig();
			var commandService = new CommandService(serviceConfig);
			var handler = new CommandHandler(_serviceProvider, _client, commandService);
			await handler.InitializeAsync();
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
