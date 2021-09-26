using Discord;
using Discord.Commands;
using StageBot.Controller;
using StageBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class StopStageCommand : ModuleBase<SocketCommandContext>
	{
		public const string STOP = "stop";
		public const string CMD_DESC = "!stop pour arrêter la présentation sur la scène.";

		[Name(STOP)]
		[Command(STOP, RunMode = RunMode.Async)]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> StopStage()
		{
			return await ExecuteCommand();
		}

		private async Task<RuntimeResult> ExecuteCommand()
		{
			try {
				// -> not connected
				var stageChannel = Context.Guild.GetStageChannel(ContextService.IdStageChannel);
				await stageChannel.StopStageAsync();

				var message = $"*a arrêté la présentation sur la scène {stageChannel.Name}.*";
				await ReplyAsync(message);

				return new CommandResult(null, new LogMessage(LogSeverity.Info, nameof(StopStageCommand), $"Stage channel {stageChannel.Name} stopped."));
			} catch (Exception e) {
				return new CommandResult(CommandError.Exception, new LogMessage(LogSeverity.Error, nameof(StopStageCommand), $"Exception while trying to stop a channel", e));
			}
		}
	}
}
