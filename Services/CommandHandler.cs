using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using StageBot.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Services
{
	public class CommandHandler
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;

		private IServiceProvider _services;

		public CommandHandler(DiscordSocketClient client, CommandService commands)
		{
			_commands = commands;
			_client = client;
		}

		public async Task InstallCommandsAsync()
		{
			//try {
				_client.MessageReceived += HandleCommandAsync;
				_client.MessageUpdated += async (before, after, channel) => { await MessageUpdated(before, after, channel); };

				_services = null;
				//new ServiceCollection()
				//.AddTransient(typeof(MainModule))
				//.BuildServiceProvider();
				await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
			//} catch (Exception e) {
			//	await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(InstallCommandsAsync), "Error", e));
			//}
		}

		private async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
		{
			//// If the message was not in the cache, downloading it will result in getting a copy of `after`.
			//var message = await before.GetOrDownloadAsync();
			//Console.WriteLine($"{message} -> {after}");
		}

		private async Task HandleCommandAsync(SocketMessage messageParam)
		{
			//try {
				// don't process the command if it was a system message
				var message = messageParam as SocketUserMessage;
				if (message is null)
					return;

				// index of start of actual message
				int argPos = 0;
				// filter messages
				if (!message.HasCharPrefix('!', ref argPos)
					|| message.HasMentionPrefix(_client.CurrentUser, ref argPos)
					|| message.Author.IsBot)
					return;

				var context = new SocketCommandContext(_client, message);
				await Task.Run(async () => await _commands.ExecuteAsync(context, argPos, _services));
			//} catch (Exception e) {
			//	await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(HandleCommandAsync), "Error", e));
			//}
		}
	}
}
