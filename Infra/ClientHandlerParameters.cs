using StageBot.Setup;
using System;

namespace StageBot.Infra
{
	public class ClientHandlerParameters
	{
		public string BotToken;
		public IServiceProvider ServiceProvider;
		public IBotStartup BotStartup;
	}
}
