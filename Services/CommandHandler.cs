using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
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
			_client.MessageReceived += HandleCommandAsync;

			_services = new ServiceCollection().BuildServiceProvider();
			await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
		}

		private async Task HandleCommandAsync(SocketMessage messageParam)
		{
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
			await _commands.ExecuteAsync(context, argPos, _services);
		}
	}
}
