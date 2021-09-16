using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StageBot.Services;
using System;
using System.IO;
using System.Reflection;

namespace StageBot.Setup
{
	class Program
	{
		public static IConfigurationRoot Configuration { get; set; }

		static void Main(string[] args)
		{
			var secretsPath = Path.Combine(Directory.GetCurrentDirectory(), "secrets.json");

			var builder = new ConfigurationBuilder();
			var appAssembly = Assembly.GetExecutingAssembly();
			builder.AddUserSecrets(appAssembly);
			Configuration = builder.Build();

			IServiceProvider services = new ServiceCollection()
				.Configure<Secrets>(Configuration.GetSection(nameof(Secrets)))
				.AddOptions()
				.AddScoped<IBotStartup, BotStartup>()
				.BuildServiceProvider();

			IBotStartup main = services.GetService<IBotStartup>();

			try {
				main.MainAsync().GetAwaiter().GetResult();
			} catch (Exception e) {
				LoggingService.Log(new LogMessage(LogSeverity.Critical, nameof(Main), "Fatal exception", e)).GetAwaiter().GetResult();
			}
		}
	}
}
