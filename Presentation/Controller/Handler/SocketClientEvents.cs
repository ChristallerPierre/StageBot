using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Presentation.Configuration;
using StageBot.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controller.Handler
{
	public class SocketClientEvents
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _command;
		private readonly IServiceProvider _services;

		public SocketClientEvents(IServiceProvider services, DiscordSocketClient client, CommandService commands)
		{
			_client = client;
			_command = commands;
			_services = services;
		}

		// todo : maybe remove these two events
		#region

		public Task OnMessageCommandExecuted(SocketMessageCommand command)
		{
			LogService.Info(nameof(OnMessageCommandExecuted), LogService.SUCCESS);
			return Task.CompletedTask;
		}

		public Task OnUserCommandExecuted(SocketUserCommand command)
		{
			LogService.Info(nameof(OnUserCommandExecuted), LogService.SUCCESS);
			return Task.CompletedTask;
		}
		#endregion

		public async Task OnMessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
		{
			//// If the message was not in the cache, downloading it will result in getting a copy of `after`.
			//var message = await before.GetOrDownloadAsync();
			//Console.WriteLine($"{message} -> {after}");

			//return Task.CompletedTask;
		}

		public async Task OnMessageReceivedAsync(SocketMessage messageParam)
		{
			// don't process the command if it was a system message
			if (!(messageParam is SocketUserMessage message))
				return;

			// index of start of actual message
			int argPos = 0;
			// filter messages
			if (!message.HasStringPrefix(CommandMark.COMMAND_MARK, ref argPos)
				|| message.HasMentionPrefix(_client.CurrentUser, ref argPos)
				|| message.Author.IsBot)
				return;

			var context = new SocketCommandContext(_client, message);
			var commandName = GetCommandNameInInput(message);

			if (HandleInexistantCommand(commandName))
				await ReplyToUnknownCommand(message);
			else {
				var logmsg = LogService.CMD_RECEIVED + Environment.NewLine + LogService.ReadCommandContext(context, commandName);
				LogService.Info(nameof(OnMessageReceivedAsync), logmsg);
				await Task.Run(async () => await _command.ExecuteAsync(context, argPos, _services));
			}
			return;
		}

		private string GetCommandNameInInput(SocketUserMessage message)
		{
			var commandWithoutCommandMark = message.Content.Remove(0, 1);
			var splits = commandWithoutCommandMark.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return splits[0];
		}

		private bool HandleInexistantCommand(string commandName)
		{
			return !CommandDescriptionHelper.Commands.Any(cmd => cmd.Aliases.Contains(commandName));
		}

		private async Task ReplyToUnknownCommand(SocketUserMessage message)
		{
			var logMessage = $"{LogService.UNKNOWN_COMMAND} : {message.Content}";

			LogService.Info(nameof(ReplyToUnknownCommand), logMessage);
			await message.ReplyAsync(LogService.UNKNOWN_COMMAND_HELP);
		}
	}
}
