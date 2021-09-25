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

namespace StageBot.Interactor
{
	public class HelpInteractor : IHelpInteractor
	{
		//IHelpCommand _command;

		//public HelpInteractor(IHelpCommand command)
		//{
		//	_command = command;
		//}

		public async Task<CommandResult> DisplayHelpAsync(IHelpCommand command)
		{
			try {
				var message = GetHelpMessage();
				await command.ReplyAsync(message);
				return new CommandResult(null, CommandResult.SUCCESS);
			} catch (Exception e) {
				await LogService.Log(new LogMessage(LogSeverity.Error, CommandList.HELP, LogService.ERROR, e));
				return new CommandResult(CommandError.Exception, CommandResult.ERROR);
			}
		}

		private string GetHelpMessage()
		{
			var messageBuilder = new StringBuilder();
			messageBuilder.AppendLine("Commandes disponibles :");
			foreach (var commandDesc in CommandList.Commands.Values) {
				messageBuilder.AppendLine(commandDesc);
			}
			return messageBuilder.ToString();
		}
	}
}
