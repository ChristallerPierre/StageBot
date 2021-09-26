using Discord;
using Discord.Commands;
using StageBot.Services;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class ExitStageCommand : ModuleBase<SocketCommandContext>
	{
		public const string EXIT = "exit";
		public const string LEAVE = "leave";
		public const string CMD_DESC = "!exit pour faire sortir le bot du channel vocal";

		[Name(EXIT)]
		[Command(EXIT, RunMode = RunMode.Async)]
		[Alias(LEAVE)]
		[Summary(CMD_DESC)]
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
