using Presentation.Controller.Implementation;
using StageBot.Modules.JoinModule;
using System.Collections.Generic;

namespace Presentation.Controller.Handler
{
	public class CommandDescriptionHelper
	{
		public static List<CommandDescriptionHelper> Commands = new List<CommandDescriptionHelper>() {
			new CommandDescriptionHelper(HelpCommand.HELP, new []{ HelpCommand.HELP, HelpCommand.QUESTION_MARK }, HelpCommand.CMD_DESC),
			new CommandDescriptionHelper(JoinStageCommand.SCENE, new []{ JoinStageCommand.SCENE, JoinStageCommand.STAGE }, JoinStageCommand.CMD_DESC),
			new CommandDescriptionHelper(StartStageCommand.START, new []{ StartStageCommand.START }, StartStageCommand.CMD_DESC),
			//new CommandDescription(StopStageCommand.STOP, new []{ StopStageCommand.STOP }, StopStageCommand.CMD_DESC),
			new CommandDescriptionHelper(EditStageCommand.EDIT, new []{ EditStageCommand.EDIT, EditStageCommand.TITRE }, EditStageCommand.CMD_DESC),
			//new CommandDescription(JoinChannelCommand.JOIN, new []{ JoinChannelCommand.JOIN }, JoinChannelCommand.CMD_DESC),
			new CommandDescriptionHelper(ExitStageCommand.EXIT, new []{ ExitStageCommand.EXIT, ExitStageCommand.LEAVE }, ExitStageCommand.CMD_DESC),
			new CommandDescriptionHelper(TopicPlanningCommand.PLAN, new []{ TopicPlanningCommand.PLAN }, TopicPlanningCommand.CMD_DESC),
		};

		public string Name { get; }
		public string[] Aliases { get; }
		public string Description { get; }

		public CommandDescriptionHelper(string name, string[] aliases, string description)
		{
			Name = name;
			Aliases = aliases;
			Description = description;
		}
	}
}
