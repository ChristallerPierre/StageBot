using Discord;
using Discord.Commands;
using Discord.WebSocket;
using StageBot.Controller;
using StageBot.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StageBot.Infra
{
	public class DiscordClientHandler : IDiscordClientHandler, IDisposable
	{
		private DiscordSocketClient _client;
		private CancellationTokenSource _cancelTokenSource;
		private CommandHandler _commandHandler;
		private readonly ClientHandlerParameters _param;

		public DiscordClientHandler(ClientHandlerParameters param)
		{
			_param = param;
			_cancelTokenSource = new CancellationTokenSource();
		}

		public async Task Connect()
		{
			_client = new DiscordSocketClient();
			_client.Log += LogService.Log;
			_client.Connected += OnClientConnected;
			_client.Disconnected += _param.BotStartup.OnClientDisconnected;
			_client.Ready += OnClientReady;

			await _client.LoginAsync(TokenType.Bot, _param.BotToken);
			await _client.StartAsync();

			// block this task until requested
			await Task.Delay(-1, _cancelTokenSource.Token);
		}

		public async Task OnClientConnected()
		{
			var serviceConfig = new CommandServiceConfig();
			var commandService = new CommandService(serviceConfig);
			_commandHandler = new CommandHandler(_param.ServiceProvider, _client, commandService);
			await _commandHandler.InitializeAsync();
		}

		public async Task OnClientReady()
		{
			LogService.Info(nameof(OnClientReady), "Client is ready");
			// todo : have the bot say hello when starting
		}

		public void Dispose()
		{
			_commandHandler.Dispose();
			_cancelTokenSource.Cancel();
			_client.Log -= LogService.Log;
			_client.Connected -= OnClientConnected;
			_client.Disconnected -= _param.BotStartup.OnClientDisconnected;
			_client.Ready -= OnClientReady;

			_client.Dispose();
			_cancelTokenSource.Dispose();
		}
	}
}
