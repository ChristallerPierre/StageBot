using Discord.Commands;
using StageBot.Controller.Precondition;
using StageBot.Infra.Configuration;
using StageBot.Services;
using System;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class StopStageCommand : ModuleBase<SocketCommandContext>
	{
		public const string STOP = "stop";
		public const string CMD_DESC = "!stop pour arrêter la présentation sur la scène.";

		[Name(STOP)]
		[Command(STOP, RunMode = RunMode.Async)]
		[RequireUserRole(GuildRoles.WIP)]
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

				return new CommandResult(null, LogService.SUCCESS);
			} catch (Exception e) {
				LogService.Error(nameof(StartStageCommand), LogService.ERROR, e);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}
	}
}
