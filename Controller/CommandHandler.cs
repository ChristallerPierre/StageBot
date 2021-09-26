using Discord;
using Discord.Commands;
using Discord.WebSocket;
using StageBot.Modules;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StageBot.Services
{
	public class CommandHandler
	{
		private const string COMMAND_MARK = "!";

		private readonly DiscordSocketClient _client;
		private readonly CommandService _command;
		private readonly IServiceProvider _services;

		public CommandHandler(IServiceProvider services, DiscordSocketClient client, CommandService commands)
		{
			_services = services;
			_client = client;
			_command = commands;
		}

		public async Task InitializeAsync()
		{
			_client.MessageReceived += OnMessageReceivedAsync;
			_client.MessageUpdated += async (before, after, channel) => { await OnMessageUpdated(before, after, channel); };
			await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
			_command.CommandExecuted += OnCommandExecutedAsync;
			_client.MessageCommandExecuted += OnMessageCommandExecuted;
			_client.UserCommandExecuted += OnUserCommandExecuted;
		}

		private Task OnMessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
		{
			//// If the message was not in the cache, downloading it will result in getting a copy of `after`.
			//var message = await before.GetOrDownloadAsync();
			//Console.WriteLine($"{message} -> {after}");

			return Task.CompletedTask;
		}

		// todo : maybe remove these events

		private async Task OnMessageCommandExecuted(SocketMessageCommand command)
		{
			await LogService.Log(new LogMessage(LogSeverity.Info, nameof(OnMessageCommandExecuted), LogService.SUCCESS));
		}

		private async Task OnUserCommandExecuted(SocketUserCommand command)
		{
			await LogService.Log(new LogMessage(LogSeverity.Info, nameof(OnUserCommandExecuted), LogService.SUCCESS));
		}

		private async Task OnCommandExecutedAsync(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
		{
			var commandName = "command_name";
			if (commandInfo.IsSpecified)
				commandName = commandInfo.Value.Name;
			else {
				await LogService.Log(new LogMessage(
					LogSeverity.Warning,
					"OnCommandExecutedAsync",
					"No commandInfo specified"));
			}

			await LogService.Log(new LogMessage(
				LogSeverity.Info,
				nameof(OnCommandExecutedAsync),
				LogService.ReadCommandContext(context, commandName, result)));
		}

		private async Task OnMessageReceivedAsync(SocketMessage messageParam)
		{
			// don't process the command if it was a system message
			if (!(messageParam is SocketUserMessage message))
				return;

			// index of start of actual message
			int argPos = 0;
			// filter messages
			if (!message.HasStringPrefix(COMMAND_MARK, ref argPos)
				|| message.HasMentionPrefix(_client.CurrentUser, ref argPos)
				|| message.Author.IsBot)
				return;

			var context = new SocketCommandContext(_client, message);

			if (HandleInexistantCommand(message))
				await OnInexistantCommandReceived(message);
			else
				await Task.Run(async () => await _command.ExecuteAsync(context, argPos, _services));
			return;
		}

		private bool HandleInexistantCommand(SocketUserMessage message)
		{
			var commandWithoutCommandMark = message.Content.Remove(0, 1);
			var splits = commandWithoutCommandMark.Split(' ', StringSplitOptions.RemoveEmptyEntries);
			return !CommandList.Commands.ContainsKey(splits[0]);
		}

		private async Task OnInexistantCommandReceived(SocketUserMessage message)
		{
			var logMessage = $"{LogService.UNKNOWN_COMMAND} : {message.Content}";

			await LogService.Log(new LogMessage(
				LogSeverity.Info,
				nameof(OnInexistantCommandReceived),
				logMessage));
			await message.ReplyAsync(LogService.UNKNOWN_COMMAND_HELP);
		}
	}
}
