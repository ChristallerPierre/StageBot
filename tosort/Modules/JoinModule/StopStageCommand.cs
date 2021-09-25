using Discord;
using Discord.Commands;
using StageBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class StopStageCommand : ModuleBase<SocketCommandContext>
	{
		// todo : update les summary des commands

		[Command(CommandList.STOP, RunMode = RunMode.Async)]
		[Summary("Demande au bot de rejoindre la scène")]
		[Name(CommandList.STOP)]
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
