using Discord.Commands;
using Presentation.Configuration;
using Presentation.Controller.Attribute;
using Presentation.Controller.Interface;
using Presentation.Interactor.Interface;
using System.Threading.Tasks;

namespace Presentation.Controller.Implementation
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
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Help()
		{
			return await _interactor.DisplayHelpAsync(this);
		}
	}
}
