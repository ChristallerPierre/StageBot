using Discord.Commands;
using Presentation.Configuration;
using Presentation.Controller.Attribute;
using Presentation.Controller.Interface;
using Presentation.Interactor.Interface;
using System.Threading.Tasks;

namespace Presentation.Controller.Implementation
{
	public class TopicPlanningCommand : BaseCommand, ITopicPlanningCommand
	{
		public const string PLAN = "plan";
		public const string CMD_DESC = "!plan list pour lister les plannifications de titre, !plan + DD/MM HH:mm <titre> pour plannifier l'affichage d'un titre, ou !plan - DD/MM HH:mm pour déplannifier l'affichage d'un titre existant.";

		ITopicPlanningInteractor _interactor;

		public TopicPlanningCommand(ITopicPlanningInteractor interactor)
		{
			_interactor = interactor;
		}

		[Name(PLAN)]
		[Command(PLAN)]
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Planning(string action, string date = null, string hour = null, [Remainder] string title = null)
		{
			return await _interactor.DispatchPlanningAsync(this, action, date, hour, title);
		}
	}
}
