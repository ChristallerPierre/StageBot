using Discord;
using Discord.Commands;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

using LogSeri = Serilog.Log;

namespace StageBot.Services
{
	public static class LoggingService
	{
		public static void Setup()
		{
			using var logger = new LoggerConfiguration()
				.WriteTo.File("Logs/Log.txt", rollingInterval: RollingInterval.Day)
				.WriteTo.Console()
				.CreateLogger();

			LogSeri.Logger = logger;
		}

		public static Task Log(LogMessage msg)
		{
			var log = string.Empty;
			if (msg.Exception is CommandException cmdException)
				log = $"{cmdException.Command.Aliases.First()} failed to execute in {cmdException.Context.Channel}.{Environment.NewLine}";

			switch (msg.Severity) {
				case LogSeverity.Critical:
					LogSeri.Fatal(msg.Exception, log + msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Debug:
					LogSeri.Debug(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Error:
					LogSeri.Error(msg.Exception, log + msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Info:
					LogSeri.Information(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Verbose:
					LogSeri.Verbose(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Warning:
					LogSeri.Warning(msg.Exception, log + msg.Source + Environment.NewLine + msg.Message);
					break;
			}

			//Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
