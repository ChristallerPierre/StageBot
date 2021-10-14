using Discord.Commands;
using Discord.WebSocket;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Controller.Handler;
using Presentation.Interactor;
using Presentation.Interactor.Interface;
using Presentation.Startup.Interface;
using System;
using System.IO.Abstractions;
using System.Reflection;

/// <summary>
/// Thanks to this doc : https://docs.stillu.cc/guides/introduction/intro.html (doesn't cover the functionalities of Discord.Net.Labs, which is a fork of Discord.Net)
/// </summary>
namespace Presentation.Startup
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
					//.AddLogging()
					.AddOptions()
					.AddSingleton<IBotStartup, BotStartup>()
					.AddSingleton<CommandHandler>()
					.AddSingleton<DiscordSocketClient>()
					.AddSingleton<CommandService>()
					.AddScoped<IHelpInteractor, HelpInteractor>()
					.AddScoped<ITopicPlanningInteractor, TopicPlanningInteractor>()
					.AddScoped<IFileSystem, FileSystem>()
					.BuildServiceProvider();

				IBotStartup main = services.GetService<IBotStartup>();
				main.StartDiscordHandler().GetAwaiter().GetResult();
			} catch (Exception e) {
				LogService.Fatal(nameof(Main), "Fatal exception", e);
			}
		}
	}
}
