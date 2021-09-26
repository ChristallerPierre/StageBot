using Discord.Commands;
using StageBot.Controller.Precondition;
using StageBot.Infra.Configuration;
using StageBot.Interactor;
using System.Threading.Tasks;

namespace StageBot.Controller.HelpModule
{
	public class HelpCommand : BaseCommand, IHelpCommand
	{
		public const string HELP = "help";
		public const string QUESTION_MARK = "?";
		public const string CMD_DESC = "!? ou !help pour afficher ce message.";

		IHelpInteractor _interactor;

		public HelpCommand(IHelpInteractor interactor)
		{
			_interactor = interactor;
		}

		[Name(HELP)]
		[Command(HELP)]
		[Alias(QUESTION_MARK)]
		[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Help()
		{
			return await _interactor.DisplayHelpAsync(this);
		}
	}
}
