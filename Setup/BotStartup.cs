using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using StageBot.Controller;
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
			_client.Log += LogService.Log;
			_client.Connected += OnClientConnected;
			_client.Disconnected += OnClientDisconnected;
			_client.Ready += OnClientReady;

			await _client.LoginAsync(TokenType.Bot, _botToken);
			await _client.StartAsync();

			// block this task until program is closed
			await Task.Delay(-1);
		}

		public async Task OnClientConnected()
		{
			var serviceConfig = new CommandServiceConfig();
			var commandService = new CommandService(serviceConfig);
			var handler = new CommandHandler(_serviceProvider, _client, commandService);
			await handler.InitializeAsync();
		}

		public Task OnClientDisconnected(Exception exception)
		{
			LogService.Warn(nameof(OnClientDisconnected), "Bot disconnected", exception);
			return Task.CompletedTask;
		}

		public async Task OnClientReady()
		{
			LogService.Info(nameof(OnClientReady), "Client is ready");
			// todo : have the bot say hello when starting
		}
	}
}
