using Discord.Commands;
using StageBot.Controller.PlanningModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Interactor
{
	public interface ITopicPlanningInteractor
	{
		Task<RuntimeResult> DispatchPlanningAsync(ITopicPlanningCommand command,string action, string date = null, string hour = null, string title = null);
	}
}
