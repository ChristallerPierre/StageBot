using StageBot.Controller.HelpModule;
using StageBot.Controller.PlanningModule;
using StageBot.Modules.JoinModule;
using System.Collections.Generic;

namespace StageBot.Controller
{
	public class CommandDescription
	{
		public static List<CommandDescription> Commands = new List<CommandDescription>() {
			new CommandDescription(HelpCommand.HELP, new []{ HelpCommand.HELP, HelpCommand.QUESTION_MARK }, HelpCommand.CMD_DESC),
			new CommandDescription(JoinStageCommand.SCENE, new []{ JoinStageCommand.SCENE, JoinStageCommand.STAGE }, JoinStageCommand.CMD_DESC),
			new CommandDescription(StartStageCommand.START, new []{ StartStageCommand.START }, StartStageCommand.CMD_DESC),
			//new CommandDescription(StopStageCommand.STOP, new []{ StopStageCommand.STOP }, StopStageCommand.CMD_DESC),
			new CommandDescription(EditStageCommand.EDIT, new []{ EditStageCommand.EDIT, EditStageCommand.TITRE }, EditStageCommand.CMD_DESC),
			//new CommandDescription(JoinChannelCommand.JOIN, new []{ JoinChannelCommand.JOIN }, JoinChannelCommand.CMD_DESC),
			new CommandDescription(ExitStageCommand.EXIT, new []{ ExitStageCommand.EXIT, ExitStageCommand.LEAVE }, ExitStageCommand.CMD_DESC),
			new CommandDescription(TopicPlanningCommand.PLAN, new []{ TopicPlanningCommand.PLAN }, TopicPlanningCommand.CMD_DESC),
		};

		public string Name { get; }
		public string[] Aliases { get; }
		public string Description { get; }

		public CommandDescription(string name, string[] aliases, string description)
		{
			Name = name;
			Aliases = aliases;
			Description = description;
		}
	}
}
