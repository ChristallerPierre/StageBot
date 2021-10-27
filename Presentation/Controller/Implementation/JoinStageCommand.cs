using Discord.Commands;
using Discord.WebSocket;
using Domain.Model;
using Infrastructure.Services;
using Presentation.Configuration;
using Presentation.Controller.Attribute;
using Presentation.Controller.Handler;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class JoinStageCommand : ModuleBase<SocketCommandContext>
	{
		public const string STAGE = "stage";
		public const string SCENE = "scene";
		public const string CMD_DESC = "!scene <nom de la scène> ou !stage <nom de la scène> pour que le bot rejoigne le channel de la scène (si précisée).";

		[Name(SCENE)]
		[Command(STAGE, RunMode = RunMode.Async)]
		[Alias(SCENE)]
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Stage(string inputStageName)
		{
			return await ExecuteCommand(inputStageName);
		}

		[Name(SCENE)]
		[Command(STAGE, RunMode = RunMode.Async)]
		[Alias(SCENE)]
		[RequireChannel(GuildChannel.BOT_CHAN)]
		//[RequireUserRole(new[] { GuildRoles.PAMPA_MOD, GuildRoles.PAMPA_RL })]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Stage()
		{
			return await ExecuteCommand();
		}

		private async Task<RuntimeResult> ExecuteCommand(string inputStageName = "")
		{
			try {
				var stageChannel = GetStageChannel(inputStageName);
				var audio = await stageChannel.ConnectAsync();
				ContextService.IdStageChannel = stageChannel.Id;
				audio.Disconnected += OnAudioDisconnected;

				var message = $"*a rejoint la scène {stageChannel.Name}.*";
				await ReplyAsync(message);

				await stageChannel.BecomeSpeakerAsync();
				return new CommandResult(null, LogService.SUCCESS);
			} catch (Exception e) {
				// todo : handle System.Threading.Tasks.TaskCanceledException: A task was canceled.
				//		at Discord.WebSocket.SocketGuild.ConnectAudioAsync(UInt64 channelId, Boolean selfDeaf, Boolean selfMute, Boolean external)

				LogService.Error(nameof(JoinStageCommand), LogService.ERROR, e);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}

		// todo: move to business part of leaving a stage
		private async Task OnAudioDisconnected(Exception e)
		{
			ContextService.IdStageChannel = 0;
			LogService.Error(nameof(OnAudioDisconnected), LogService.ERROR, e);
		}

		private SocketStageChannel GetStageChannel(string inputStageName)
		{
			// todo : add error cases

			SocketStageChannel stage;
			if (string.IsNullOrWhiteSpace(inputStageName))
				stage = Context.Guild.StageChannels.First();
			else
				stage = Context.Guild.StageChannels.First(stage => stage.Name.ToLower().StartsWith(inputStageName));
			return stage;
		}
	}
}
