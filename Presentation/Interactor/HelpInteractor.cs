using Discord.Commands;
using StageBot.Controller;
using StageBot.Controller.HelpModule;
using StageBot.Modules;
using StageBot.Services;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Interactor
{
	public class HelpInteractor : IHelpInteractor
	{
		public async Task<CommandResult> DisplayHelpAsync(IHelpCommand command)
		{
			try {
				var message = GetHelpMessage();
				await command.ReplyAsync(message);
				return new CommandResult(null, LogService.SUCCESS);
			} catch (Exception e) {
				LogService.Error(nameof(DisplayHelpAsync), LogService.ERROR, e);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
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
