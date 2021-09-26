using Discord.Commands;
using System.Linq;
using StageBot.Modules;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StageBot.Services;
using Discord;
using StageBot.Controller.HelpModule;
using StageBot.Controller;

namespace StageBot.Interactor
{
	public class HelpInteractor : IHelpInteractor
	{
		public async Task<CommandResult> DisplayHelpAsync(IHelpCommand command)
		{
			try {
				var message = GetHelpMessage();
				await command.ReplyAsync(message);
				return new CommandResult(
					null,
					new LogMessage(
						LogSeverity.Info,
						nameof(DisplayHelpAsync),
						LogService.SUCCESS));
			} catch (Exception e) {
				var logMessage = new LogMessage(LogSeverity.Error, nameof(DisplayHelpAsync), LogService.ERROR, e);
				await LogService.Log(logMessage);
				return new CommandResult(CommandError.Exception, logMessage);
			}
		}

		private string GetHelpMessage()
		{
			var messageBuilder = new StringBuilder();
			messageBuilder.AppendLine("Commandes disponibles :");
			foreach (var commandDesc in CommandDescription.Commands.Select(cmd => cmd.Description)) {
				messageBuilder.AppendLine(commandDesc);
			}
			return messageBuilder.ToString();
		}
	}
}
