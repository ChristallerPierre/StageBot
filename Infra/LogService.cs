using Discord;
using Discord.Commands;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;

using LogSeri = Serilog.Log;

namespace StageBot.Services
{
	public static class LogService
	{
		public const string ERROR = "Error";

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
			switch (msg.Severity) {
				case LogSeverity.Critical:
					LogSeri.Fatal(msg.Exception, GetLogMessage(msg));
					break;
				case LogSeverity.Debug:
					LogSeri.Debug(msg.Exception, GetLogMessage(msg));
					break;
				case LogSeverity.Error:
					LogSeri.Error(msg.Exception, GetLogMessage(msg));
					break;
				case LogSeverity.Info:
					LogSeri.Information(msg.Exception, GetLogMessage(msg));
					break;
				case LogSeverity.Verbose:
					LogSeri.Verbose(msg.Exception, GetLogMessage(msg));
					break;
				case LogSeverity.Warning:
					LogSeri.Warning(msg.Exception, GetLogMessage(msg));
					break;
			}

			return Task.CompletedTask;
		}

		private static string GetLogMessage(LogMessage msg)
		{
			var log = string.Empty;
			if (msg.Exception is CommandException cmdException)
				log = $"{cmdException.Command.Aliases.First()} failed to execute in {cmdException.Context.Channel}.{Environment.NewLine}";
			return log + msg.Source + Environment.NewLine + msg.Message;
		}
	}
}
