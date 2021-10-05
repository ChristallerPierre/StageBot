﻿using Microsoft.Extensions.Options;
using StageBot.Infra;
using StageBot.Services;
using System;
using System.Threading.Tasks;

namespace StageBot.Setup
{
	public class BotStartup : IBotStartup, IDisposable
	{
		private IDiscordClientHandler _clientHandler;
		private readonly ClientHandlerParameters _handlerParameters;

		public BotStartup(IOptions<Secrets> secrets, IServiceProvider serviceProvider)
		{
			_handlerParameters = new ClientHandlerParameters() {
				BotToken = secrets.Value.BotToken,
				ServiceProvider = serviceProvider,
			};
		}

		public async Task StartDiscordHandler()
		{
			_clientHandler = new DiscordClientHandler(_handlerParameters);
			if (!await _clientHandler.Connect()) {
				_clientHandler.Dispose();
				await StartDiscordHandler();
			}
		}

		public void Dispose()
		{
			_clientHandler?.Dispose();
		}
	}
}
