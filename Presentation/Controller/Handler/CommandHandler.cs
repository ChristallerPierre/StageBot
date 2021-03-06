using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Infrastructure.Services;
using Presentation.Helper;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Presentation.Controller.Handler
{
	public class CommandHandler : IDisposable
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _command;
		private readonly IServiceProvider _services;
		private readonly SocketClientEvents _socketClientEvents;

		public CommandHandler(IServiceProvider services, DiscordSocketClient client, CommandService commands)
		{
			_services = services;
			_client = client;
			_command = commands;
			_socketClientEvents = new SocketClientEvents(_services, _client, _command);
		}

		public async Task InitializeAsync()
		{
			_client.MessageReceived += _socketClientEvents.OnMessageReceivedAsync;
			_client.MessageUpdated += _socketClientEvents.OnMessageUpdated;
			await _command.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
			_command.CommandExecuted += OnCommandExecutedAsync;
			_client.MessageCommandExecuted += _socketClientEvents.OnMessageCommandExecuted;
			_client.UserCommandExecuted += _socketClientEvents.OnUserCommandExecuted;
		}

		public void Dispose()
		{
			_client.MessageReceived -= _socketClientEvents.OnMessageReceivedAsync;
			_client.MessageUpdated -= _socketClientEvents.OnMessageUpdated;
			_command.CommandExecuted -= OnCommandExecutedAsync;
			_client.MessageCommandExecuted -= _socketClientEvents.OnMessageCommandExecuted;
			_client.UserCommandExecuted -= _socketClientEvents.OnUserCommandExecuted;
			_client.Dispose();
		}

		private Task OnCommandExecutedAsync(Optional<CommandInfo> commandInfo, ICommandContext context, IResult result)
		{
			var commandName = "command_name";
			//if (commandInfo.IsSpecified)
			//{
			commandName = commandInfo.Value.Name;
			var logmsg = LogService.CMD_EXECUTED + Environment.NewLine + LogHelper.ReadCommandContext(context, commandName, result);
			LogService.Info(nameof(OnCommandExecutedAsync), logmsg);
			//} else
			//	LogService.Warn(nameof(OnCommandExecutedAsync), LogService.MISSING_CMD);
			return Task.CompletedTask;
		}
	}
}
