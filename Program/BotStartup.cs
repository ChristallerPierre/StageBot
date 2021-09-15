using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace StageBot
{
	public class BotStartup : IBotStartup
	{
		private string _botToken;
		private DiscordSocketClient _client;

		public BotStartup(IOptions<Secrets> secrets)
		{
			_botToken = secrets.Value.BotToken;
		}

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_client.Log += Log;
			await _client.LoginAsync(TokenType.Bot, _botToken);
			await _client.StartAsync();
			// block this task until program is closed
			await Task.Delay(-1);
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
