using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace StageBot.Services
{
	public class CommandHandler
	{
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
		}

		private async Task OnMessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
		{
			//// If the message was not in the cache, downloading it will result in getting a copy of `after`.
			//var message = await before.GetOrDownloadAsync();
			//Console.WriteLine($"{message} -> {after}");
		}

		private async Task OnCommandExecutedAsync(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
		{
			// We have access to the information of the command executed,
			// the context of the command, and the result returned from the
			// execution in this event.

			var commandName = "command_name";
			if (commandInfo.IsSpecified)
				commandName = commandInfo.Value.Name;
			else {
				await LoggingService.Log(new LogMessage(
					LogSeverity.Warning,
					"OnCommandExecutedAsync",
					"No commandInfo specified"));
			}
			var channel = context.Channel.Name;
			var user = context.User.Username + "#" + context.User.Discriminator;
			var message = context.Message.Content;
			var guild = context.Guild.Name;

			await LoggingService.Log(new LogMessage(
				LogSeverity.Info,
				"OnCommandExecutedAsync",
				$"Guild {guild} ; Command {commandName} ; Channel {channel} ; User {user} ; Message \"{message}\""));
		}

		private async Task OnMessageReceivedAsync(SocketMessage messageParam)
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
			//await
			Task.Run(async () => await _command.ExecuteAsync(context, argPos, _services));
			//} catch (Exception e) {
			//	await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(HandleCommandAsync), "Error", e));
			//}
		}
	}
}
