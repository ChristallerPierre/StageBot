using Discord;
using Discord.Commands;
using Serilog;
using StageBot.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

using LogSeri = Serilog.Log;

namespace StageBot.Services
{
	public static class LogService
	{
		public const string ERROR = "Error";
		public const string SUCCESS = "Success";
		public const string BAD_USAGE = "Bad usage";
		public const string CHANNEL_NOT_FOUND = "Channel not found";
		public const string UNKNOWN_COMMAND = "Unknown comand";
		public const string UNKNOWN_COMMAND_HELP = "Commande non-reconnue. !help pour afficher la liste des commandes";
		public const string MISING_CHANNEL_NAME = "Veuillez exécuter la commande en étant connecté à un channel vocal, ou préciser le nom d'un channel après la commande.";
		public const string CMD_RECEIVED = "Command received : ";
		public const string MISSING_CMD = "No commandInfo specified";
		public const string CMD_EXECUTED = "Command executed : ";
		public const string TOPIC_UPDATED = "Topic updated";

		public static string ReadCommandContext(ICommandContext context, string commandName, IResult result = null)
		{
			var channel = context.Channel.Name;
			var usertag = UserHelper.GetUserTag(context);
			var messageContent = context.Message.Content;
			//var guild = context.Guild.Name;
			var logResult = result is null ? string.Empty : ReadResult(result);

			//Guild {guild}
			return @$"{logResult}Channel {channel} ; User {usertag} ; Message {messageContent}";
		}

		private static string ReadResult(IResult result)
		{
			return
@$"Success {result.IsSuccess} ; ReturnCode {result.Error} ; ReturnMessage {result.ErrorReason} ; 
";
		}

		public static void Setup()
		{
			using var logger = new LoggerConfiguration()
				.WriteTo.File("Logs/Log.txt", rollingInterval: RollingInterval.Day)
				.WriteTo.Console()
				.CreateLogger();

			LogSeri.Logger = logger;
		}

		public static void Info(string source, string message)
		{
			Log(new LogMessage(LogSeverity.Info, source, message));
		}

		public static void Warn(string source, string message, Exception exception = null)
		{
			Log(new LogMessage(LogSeverity.Warning, source, message, exception));
		}

		public static void Error(string source, string message, Exception exception = null)
		{
			Log(new LogMessage(LogSeverity.Error, source, message, exception));
		}

		public static void Fatal(string source, string message, Exception exception = null)
		{
			Log(new LogMessage(LogSeverity.Critical, source, message, exception));
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
