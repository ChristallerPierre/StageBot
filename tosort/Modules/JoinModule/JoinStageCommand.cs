﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using StageBot.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class JoinStageCommand : ModuleBase<SocketCommandContext>
	{
		public const string STAGE = "stage";
		public const string SCENE = "scene";
		public const string CMD_DESC = "!scene <nom de la scène> pour que le bot rejoigne le channel de la scène précisée.";

		[Name(SCENE)]
		[Command(STAGE, RunMode = RunMode.Async)]
		[Alias(SCENE)]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Stage(string inputStageName)
		{
			return await ExecuteCommand(inputStageName);
		}

		[Name(SCENE)]
		[Command(STAGE, RunMode = RunMode.Async)]
		[Alias(SCENE)]
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
				return new CommandResult(
					null,
					new LogMessage(
						LogSeverity.Info,
						nameof(JoinStageCommand),
						LogService.SUCCESS));
			} catch (Exception e) {
				var logMessage = new LogMessage(LogSeverity.Error, nameof(JoinStageCommand), LogService.ERROR, e);
				await LogService.Log(logMessage);
				return new CommandResult(CommandError.Exception, logMessage);
			}
		}

		// todo: move to business part of leaving a stage
		private async Task OnAudioDisconnected(Exception e)
		{
			ContextService.IdStageChannel = 0;
			await LogService.Log(new LogMessage(LogSeverity.Error, nameof(OnAudioDisconnected), LogService.ERROR, e));
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
