﻿using Discord;
using Discord.Commands;
using StageBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Modules
{
	public class HelpCommand : ModuleBase<SocketCommandContext>
	{
		[Command(Commands.HELP)]
		[Alias(Commands.QUESTION_MARK)]
		[Summary("Affiche l'aide")]
		[Name(Commands.HELP)]
		public async Task<RuntimeResult> Help()
		{
			try {
				var messageBuilder = new StringBuilder();
				messageBuilder.AppendLine("Commandes disponibles :");

				// todo  : Commands n'est pas un enum, appeler chacune de ses propriétés

				foreach (var command in Enum.GetNames(typeof(Commands))) {
					messageBuilder.Append("!");
					messageBuilder.AppendLine(command.ToString());
				}
				var message = messageBuilder.ToString();

				await ReplyAsync(message);
				return new CommandResult(null, CommandResult.SUCCESS);
			} catch (Exception e) {
				await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(Help), LoggingService.ERROR, e));
				return new CommandResult(CommandError.ParseFailed, CommandResult.ERROR);
			}
		}
	}
}