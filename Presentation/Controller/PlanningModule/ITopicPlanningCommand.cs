using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Controller.PlanningModule
{
	public interface ITopicPlanningCommand
	{
		Task<IUserMessage> ReplyAsync(string text);
	}
}
