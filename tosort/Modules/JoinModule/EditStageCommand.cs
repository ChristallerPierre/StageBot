using Discord.Commands;
using StageBot.Controller.Precondition;
using StageBot.Infra.Configuration;
using StageBot.Services;
using System;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class EditStageCommand : ModuleBase<SocketCommandContext>
	{
		public const string EDIT = "edit";
		public const string TITRE = "titre";
		public const string CMD_DESC = "!edit <nouveau titre> ou !titre <nouveau titre> pour changer le titre de la présentation.";

		[Name(EDIT)]
		[Command(EDIT, RunMode = RunMode.Async)]
		[Alias(TITRE)]
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> EditTopic(string param)
		{
			return await ExecuteCommand(param);
		}

		public async Task<RuntimeResult> ExecuteCommand(string param)
		{
			try {
				// handle params without quotes
				var stageChannel = Context.Guild.GetStageChannel(ContextService.IdStageChannel);
				await stageChannel.ModifyInstanceAsync(prop => prop.Topic = param);

				var message = $"*a changé le sujet de la scène {stageChannel.Name} en {param}.*";
				await ReplyAsync(message);

				return new CommandResult(null, LogService.TOPIC_UPDATED);
			} catch (Exception ex) {
				LogService.Error(nameof(EditStageCommand), LogService.ERROR, ex);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}
	}
}
