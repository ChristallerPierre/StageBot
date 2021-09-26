using Discord;
using Discord.Commands;
using StageBot.Services;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class EditStageCommand : ModuleBase<SocketCommandContext>
	{
		public const string EDIT = "edit";
		public const string TITRE = "titre";
		public const string CMD_DESC = "!titre <nouveau titre> pour changer le titre de la présentation.";

		[Name(EDIT)]
		[Command(EDIT, RunMode = RunMode.Async)]
		[Alias(TITRE)]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> EditTopic(string param)
		{
			return await ExecuteCommand(param);
		}

		public async Task<RuntimeResult> ExecuteCommand(string param)
		{
			// todo trycatch
			// handle params without quotes
			var stageChannel = Context.Guild.GetStageChannel(ContextService.IdStageChannel);
			await stageChannel.ModifyInstanceAsync(prop => prop.Topic = param);

			var message = $"*a changé le sujet de la scène {stageChannel.Name} en {param}.*";
			await ReplyAsync(message);

			return new CommandResult(null, new LogMessage(LogSeverity.Info, nameof(EditStageCommand), $"Topic of channel {stageChannel.Name} updated to {param}"));
		}
	}
}
