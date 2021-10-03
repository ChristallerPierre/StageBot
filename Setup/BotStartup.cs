using Microsoft.Extensions.Options;
using StageBot.Infra;
using StageBot.Services;
using System;
using System.Threading.Tasks;

namespace StageBot.Setup
{
	public class BotStartup : IBotStartup
	{
		private ClientHandlerParameters _handlerParameters;
		private IDiscordClientHandler _clientHandler;

		public BotStartup(IOptions<Secrets> secrets, IServiceProvider serviceProvider)
		{
			_handlerParameters = new ClientHandlerParameters() {
				BotStartup = this,
				BotToken = secrets.Value.BotToken,
				ServiceProvider = serviceProvider,
			};
		}

		public async Task MainAsync()
		{
			_clientHandler = new DiscordClientHandler(_handlerParameters);
			await _clientHandler.Connect();
		}

		public async Task OnClientDisconnected(Exception exception)
		{
			LogService.Warn(nameof(OnClientDisconnected), "Bot disconnected", exception);
			_clientHandler.Dispose();
			await MainAsync();
		}
	}
}
