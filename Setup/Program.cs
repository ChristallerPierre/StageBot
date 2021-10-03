using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StageBot.Controller;
using StageBot.Interactor;
using StageBot.Services;
using System;
using System.Reflection;

/// <summary>
/// Thanks to this doc : https://docs.stillu.cc/guides/introduction/intro.html (doesn't cover the functionalities of Discord.Net.Labs, which is a fork of Discord.Net)
/// </summary>
namespace StageBot.Setup
{
	class Program
	{
		static void Main(string[] args)
		{
			LogService.Setup();
			try {
				var configBuilder = new ConfigurationBuilder();
				var appAssembly = Assembly.GetExecutingAssembly();
				configBuilder.AddUserSecrets(appAssembly);
				var configuration = configBuilder.Build();

				IServiceProvider services = new ServiceCollection()
					.Configure<Secrets>(configuration.GetSection(nameof(Secrets)))
					.AddOptions()
					.AddSingleton<IBotStartup, BotStartup>()
					.AddSingleton<CommandHandler>()
					.AddSingleton<DiscordSocketClient>()
					.AddSingleton<CommandService>()
					.AddScoped<IHelpInteractor, HelpInteractor>()
					.BuildServiceProvider();

				IBotStartup main = services.GetService<IBotStartup>();
				main.MainAsync().GetAwaiter().GetResult();
			} catch (Exception e) {
				LogService.Fatal(nameof(Main), "Fatal exception", e);
			}
		}
	}
}
