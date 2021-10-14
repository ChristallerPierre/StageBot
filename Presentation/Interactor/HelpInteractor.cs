using Discord.Commands;
using Infrastructure.Services;
using Presentation.Controller.Handler;
using Presentation.Controller.Interface;
using Presentation.Interactor.Interface;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Interactor
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
			foreach (var commandDesc in CommandDescriptionHelper.Commands.Select(cmd => cmd.Description)) {
				messageBuilder.AppendLine(commandDesc);
			}
			return messageBuilder.ToString();
		}
	}
}
