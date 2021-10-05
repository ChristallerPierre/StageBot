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

		public async Task<bool> Connect()
		{
			try {
				_client = new DiscordSocketClient();
				_client.Log += LogService.Log;
				_client.Connected += OnClientConnected;
				_client.Disconnected += OnClientDisconnected;
				_client.Ready += OnClientReady;

				await _client.LoginAsync(TokenType.Bot, _param.BotToken);
				await _client.StartAsync();

				// block this task until requested
				await Task.Delay(-1, _cancelTokenSource.Token);
				return true;
			} catch (Exception ex) {
				LogService.Error(nameof(DiscordClientHandler), "Error in Connect", ex);
				return false;
			}
		}

		public Task OnClientDisconnected(Exception exception)
		{
			LogService.Warn(nameof(OnClientDisconnected), "Bot disconnected", exception);
			return Task.CompletedTask;
		}

		public async Task OnClientConnected()
		{
			var serviceConfig = new CommandServiceConfig();
			var commandService = new CommandService(serviceConfig);
			_commandHandler = new CommandHandler(_param.ServiceProvider, _client, commandService);
			await _commandHandler.InitializeAsync();
		}

		public Task OnClientReady()
		{
			LogService.Info(nameof(OnClientReady), "Client is ready");
			// todo : have the bot say hello when starting
			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_cancelTokenSource.Cancel();
			_client.Log -= LogService.Log;
			_client.Connected -= OnClientConnected;
			_client.Disconnected -= OnClientDisconnected;
			_client.Ready -= OnClientReady;

			_commandHandler?.Dispose();
			_client?.Dispose();
			_cancelTokenSource?.Dispose();
		}
	}
}
