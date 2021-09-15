using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Serilog;
using System;
using System.Threading.Tasks;

namespace StageBot
{
	class Program
	{
		private string _botToken;
		private DiscordSocketClient _client;

		static void Main(string[] args)
		{
			try {
				new Program().MainAsync().GetAwaiter().GetResult();
			} catch (Exception e) {
				Log(new LogMessage(LogSeverity.Critical, nameof(Main), "Fatal exception", e)).GetAwaiter().GetResult();
			}
		}

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_client.Log += Log;
			Environment.
			new BaseDiscordClient();
		}

		public static Task Log(LogMessage msg)
		{
			switch (msg.Severity) {
				case LogSeverity.Critical:
					Serilog.Log.Fatal(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Debug:
					Serilog.Log.Debug(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Error:
					Serilog.Log.Error(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Info:
					Serilog.Log.Information(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Verbose:
					Serilog.Log.Verbose(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
				case LogSeverity.Warning:
					Serilog.Log.Warning(msg.Exception, msg.Source + Environment.NewLine + msg.Message);
					break;
			}

			Console.WriteLine(msg.ToString());

			return Task.CompletedTask;
		}
	}
}
