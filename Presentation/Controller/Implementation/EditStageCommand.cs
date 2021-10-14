using Discord.Commands;
using Domain.Model;
using Domain.Services;
using Presentation.Configuration;
using Presentation.Controller.Attribute;
using Presentation.Controller.Handler;
using Presentation.Controller.Interface;
using Presentation.Interactor.Interface;
using System;
using System.Threading.Tasks;

namespace Presentation.Controller.Implementation
{
	public class EditStageCommand : BaseCommand, IEditStageCommand
	{
		public const string EDIT = "edit";
		public const string TITRE = "titre";
		public const string CMD_DESC = "!edit <nouveau titre> ou !titre <nouveau titre> pour changer le titre de la présentation.";

		IEditStageInteractor _interactor;

		public EditStageCommand(IEditStageInteractor interactor)
		{
			_interactor = interactor;
		}

		[Name(EDIT)]
		[Command(EDIT, RunMode = RunMode.Async)]
		[Alias(TITRE)]
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> EditTopic([Remainder] string param)
		{
			var stageChannel = Context.Guild.GetStageChannel(ContextService.IdStageChannel);
			return await _interactor.EditTopicAsync(this, param, stageChannel);
		}
	}
}
