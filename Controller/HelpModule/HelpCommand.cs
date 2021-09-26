using Discord;
using Discord.Commands;
using StageBot.Controller.Precondition;
using StageBot.Infra.Configuration;
using StageBot.Interactor;
using StageBot.Modules;
using System.Threading.Tasks;

namespace StageBot.Controller.HelpModule
{
	public class HelpCommand : BaseCommand, IHelpCommand
	{
		IHelpInteractor _interactor;

		public HelpCommand(IHelpInteractor interactor)
		{
			_interactor = interactor;
		}

		[Command(CommandList.HELP)]
		[Alias(CommandList.QUESTION_MARK)]
		[Summary("Affiche l'aide")]
		[Name(CommandList.HELP)]
		[RequireRole(GuildRoles.ROLE)]
		public async Task<RuntimeResult> Help()
		{
			return await _interactor.DisplayHelpAsync(this);
		}
	}
}
