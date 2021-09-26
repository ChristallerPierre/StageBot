using Discord;
using Discord.Commands;
using StageBot.Controller.Precondition;
using StageBot.Infra.Configuration;
using StageBot.Services;
using System;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class StartStageCommand : ModuleBase<SocketCommandContext>
	{
		public const string START = "start";
		public const string CMD_DESC = "!start <titre> pour démarrer la présentation sur la scène.";

		[Name(START)]
		[Command(START, RunMode = RunMode.Async)]
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> StartStage(string inputTopic)
		{
			return await ExecuteCommand(inputTopic);
		}

		private async Task<RuntimeResult> ExecuteCommand(string inputTopic)
		{
			try {
				// todo : handle error case
				// -> topic empty
				// -> not connected
				var stageChannel = Context.Guild.GetStageChannel(ContextService.IdStageChannel);
				await stageChannel.StartStageAsync(inputTopic, StagePrivacyLevel.GuildOnly);

				var message = $"*a démarré la présentation sur la scène {stageChannel.Name}.*";
				await ReplyAsync(message);

				return new CommandResult(null, LogService.SUCCESS);
			} catch (Exception ex) {
				LogService.Error(nameof(StartStageCommand), LogService.ERROR, ex);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}
	}
}
