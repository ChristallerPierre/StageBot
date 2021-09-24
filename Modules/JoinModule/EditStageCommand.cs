using Discord;
using Discord.Commands;
using StageBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class EditStageCommand : ModuleBase<SocketCommandContext>
	{
		[Command(Commands.EDIT, RunMode = RunMode.Async)]
		[Summary("Demande au bot de rejoindre la scène")]
		[Name(Commands.EDIT)]
		[Alias(Commands.TITLE)]
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
