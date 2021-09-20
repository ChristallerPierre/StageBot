using Discord;
using Discord.Commands;
using StageBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Modules
{
	public class HelpModule : ModuleBase<SocketCommandContext>
	{
		[Command(Commands.HELP)]
		[Alias(Commands.QUESTION_MARK)]
		[Summary("Affiche l'aide")]
		[Name(Commands.HELP)]
		public async Task Help()
		{
			try {
				var messageBuilder = new StringBuilder();
				messageBuilder.AppendLine("Commandes disponibles :");

				foreach (var command in Enum.GetNames(typeof(Commands))) {
					messageBuilder.Append("!");
					messageBuilder.AppendLine(command.ToString());
				}
				var message = messageBuilder.ToString();

				await ReplyAsync(message);
			} catch (Exception e) {
				await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(Help), "Error", e));
			}
		}
	}
}
