using Discord.Commands;
using StageBot.Controller.Precondition;
using StageBot.Infrastructure.Configuration;
using StageBot.Interactor;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Controller.PlanningModule
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
