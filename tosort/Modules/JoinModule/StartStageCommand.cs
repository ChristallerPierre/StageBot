using Discord;
using Discord.Commands;
using StageBot.Controller.Precondition;
using StageBot.Infra.Configuration;
using StageBot.Services;
using System;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	// todo handle Discord.Net.HttpException: The server responded with error 150006: The Stage is already open

	public class StartStageCommand : ModuleBase<SocketCommandContext>
	{
		public const string START = "start";
		public const string CMD_DESC = "!start <titre> pour démarrer la présentation sur la scène.";
		public const string ERR_PARAM_NB = "Veuillez faire la commande du titre de la conférence";

		[Name(START)]
		[Command(START, RunMode = RunMode.Async)]
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> StartStage()
		{
			await ReplyAsync(ERR_PARAM_NB);
			var result = new CommandResult(CommandError.BadArgCount, LogService.BAD_USAGE);
			return result;
		}

		[Name(START)]
		[Command(START, RunMode = RunMode.Async)]
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> StartStage([Remainder] string inputTopic)
		{
			return await ExecuteCommand(inputTopic);
		}

		private async Task<RuntimeResult> ExecuteCommand(string inputTopic)
		{
			try {
				var stageChannel = Context.Guild.GetStageChannel(ContextService.IdStageChannel);

				if (IsBotConnectedToStage()) {
					await stageChannel.StartStageAsync(inputTopic, StagePrivacyLevel.GuildOnly);
					var messageSuccess = $"*a démarré la présentation sur la scène {stageChannel.Name}.*";
					await ReplyAsync(messageSuccess);
					return new CommandResult(null, LogService.SUCCESS);
				} else {
					var messageFail = $"Le bot doit rejoindre une scène (avec la commande !scene) avant de pouvoir y démarrer une conférence";
					await ReplyAsync(messageFail);
					return new CommandResult(CommandError.Unsuccessful, LogService.BAD_USAGE);
				}

			} catch (Exception ex) {
				LogService.Error(nameof(StartStageCommand), LogService.ERROR, ex);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}

		private bool IsBotConnectedToStage()
		{
			return ContextService.IdStageChannel != 0;
		}
	}
}
