﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StageBot.Services;
using System;
using System.IO;
using System.Reflection;

// todo : fix time-out sur connexion audio
// todo : ajouter gestion des rôles
// https://docs.stillu.cc/faq/commands/general.html#how-can-i-restrict-some-of-my-commands-so-only-specific-users-can-execute-them
// todo : auto disconnect from voice channels on stop
// todo : /commands

/// <summary>
/// Rien d'original, juste un peu de jugeote et pompé sans honte de 
/// https://docs.stillu.cc/guides/introduction/intro.html
/// </summary>
namespace StageBot.Setup
{
	class Program
	{
		static void Main(string[] args)
		{
			LoggingService.Setup();
			try {
				var secretsPath = Path.Combine(Directory.GetCurrentDirectory(), "secrets.json");

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
					.BuildServiceProvider();

				IBotStartup main = services.GetService<IBotStartup>();
				main.MainAsync().GetAwaiter().GetResult();
			} catch (Exception e) {
				LoggingService.Log(new LogMessage(LogSeverity.Critical, nameof(Main), "Fatal exception", e)).GetAwaiter().GetResult();
			}
		}
	}
}
