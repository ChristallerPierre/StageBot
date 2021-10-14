using Discord.Commands;
using Domain.Model;
using Infrastructure.Services;
using Presentation.Configuration;
using Presentation.Controller.Attribute;
using Presentation.Controller.Handler;
using Presentation.Controller.Interface;
using Presentation.Interactor.Interface;
using System;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class ExitStageCommand : BaseCommand, IExitStageCommand
	{
		public const string EXIT = "exit";
		public const string LEAVE = "leave";
		public const string CMD_DESC = "!exit ou !leave pour faire sortir le bot du channel vocal";

		IExitStageInteractor _interactor;

		public ExitStageCommand(IExitStageInteractor interactor)
		{
			_interactor = interactor;
		}

		[Name(EXIT)]
		[Command(EXIT, RunMode = RunMode.Async)]
		[Alias(LEAVE)]
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.WIP })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Exit()
		{
			return await ExecuteCommand();
		}

		private async Task<RuntimeResult> ExecuteCommand()
		{
			try {
				if (ContextService.IdStageChannel == 0) {
					// error
					return new CommandResult(null, LogService.ERROR);
				} else {
					var stageChannel = Context.Guild.GetStageChannel(ContextService.IdStageChannel);

					return await _interactor.Disconnect(this, stageChannel);
				}
			} catch (Exception ex) {
				LogService.Error(nameof(ExitStageCommand), LogService.ERROR, ex);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}
	}
}
