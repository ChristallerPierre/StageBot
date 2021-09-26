using Discord;
using Discord.Commands;
using StageBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class ExitStageCommand : ModuleBase<SocketCommandContext>
	{
		[Command(CommandList.EXIT, RunMode = RunMode.Async)]
		//[Summary("Demande au bot de rejoindre la scène")]
		[Name(CommandList.EXIT)]
		[Alias(CommandList.LEAVE)]
		public async Task<RuntimeResult> Exit()
		{
			return await ExecuteCommand();
		}

		private async Task<RuntimeResult> ExecuteCommand()
		{
			if (ContextService.IdStageChannel == 0) {
				// error
			} else {
				var stageChannel = Context.Guild.GetStageChannel(ContextService.IdStageChannel);
				await stageChannel.DisconnectAsync();

				var message = $"*a quitté la scène {stageChannel.Name}.*";
				await ReplyAsync(message);
			}

			return new CommandResult(null, new LogMessage(LogSeverity.Info, nameof(ExitStageCommand), "ok"));
		}
	}
}
