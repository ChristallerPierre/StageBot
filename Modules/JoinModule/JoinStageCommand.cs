﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using StageBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Modules.JoinModule
{
	public class JoinStageCommand : ModuleBase<SocketCommandContext>
	{
		[Command(Commands.STAGE, RunMode = RunMode.Async)]
		[Summary("Demande au bot de rejoindre la scène")]
		[Name(Commands.SCENE)]
		[Alias(Commands.SCENE)]
		public async Task<RuntimeResult> Stage(string inputStageName)
		{
			return await ExecuteCommand(inputStageName);
		}

		[Command(Commands.STAGE, RunMode = RunMode.Async)]
		[Summary("Demande au bot de rejoindre la scène")]
		[Name(Commands.SCENE)]
		[Alias(Commands.SCENE)]
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
				return new CommandResult(null, CommandResult.SUCCESS);
			} catch (Exception e) {
				await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(Stage), LoggingService.ERROR, e));
				return new CommandResult(CommandError.Exception, CommandResult.ERROR);
			}
		}

		// todo: move to business part of leaving a stage
		private async Task OnAudioDisconnected(Exception e)
		{
			ContextService.IdStageChannel = 0;
			await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(OnAudioDisconnected), LoggingService.ERROR, e));
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
