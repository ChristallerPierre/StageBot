using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StageBot.Services
{
	public class LoggingService
	{
		public static Task Log(LogMessage msg)
		{
			var log = string.Empty;
			if (msg.Exception is CommandException cmdException)
				log = $"{cmdException.Command.Aliases.First()} failed to execute in {cmdException.Context.Channel}.{Environment.NewLine}";

			switch (msg.Severity) {
				case LogSeverity.Critical:
					Serilog.Log.Fatal(msg.Exception, log + msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Debug:
					Serilog.Log.Debug(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Error:
					Serilog.Log.Error(msg.Exception, log + msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Info:
					Serilog.Log.Information(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Verbose:
					Serilog.Log.Verbose(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Warning:
					Serilog.Log.Warning(msg.Exception, log + msg.Source + Environment.NewLine + msg.Message);
					break;
			}

			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
