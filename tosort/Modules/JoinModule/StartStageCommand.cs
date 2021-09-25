using Discord;
using Discord.Commands;
using StageBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class StartStageCommand : ModuleBase<SocketCommandContext>
	{
		[Command(CommandList.START, RunMode = RunMode.Async)]
		[Summary("Demande au bot de rejoindre la scène")]
		[Name(CommandList.START)]
		public async Task<RuntimeResult> StartStage(string inputTopic)
		{
			return await ExecuteCommand(inputTopic);
		}

		private async Task<RuntimeResult> ExecuteCommand(string inputTopic)
		{
			// todo : handle error case
			// -> topic empty
			// -> not connected
			var stageChannel = Context.Guild.GetStageChannel(ContextService.IdStageChannel);
			await stageChannel.StartStageAsync(inputTopic, StagePrivacyLevel.GuildOnly);

			var message = $"*a démarré la présentation sur la scène {stageChannel.Name}.*";
			await ReplyAsync(message);

			return new CommandResult(null, new LogMessage(LogSeverity.Info, nameof(EditStageCommand), "ok"));
		}
	}
}
