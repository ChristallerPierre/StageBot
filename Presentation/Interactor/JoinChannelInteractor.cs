using Discord.Commands;
using Infrastructure.Services;
using Presentation.Controller.Handler;
using Presentation.Controller.Interface;
using Presentation.Interactor.Interface;
using System;
using System.Threading.Tasks;

namespace Presentation.Interactor
{
	public class JoinChannelInteractor : IJoinChannelInteractor
	{
		public async Task<CommandResult> JoinAsync(IJoinChannelCommand command, string inputChannelName = "")
		{
			try {
				var selectedChannel = await GetRequestedChannelName(command, inputChannelName);
				if (selectedChannel != null) {
					await ReplyChannelFound(command, selectedChannel);
					return await ConnectToChannel(command, selectedChannel);
				}
				return new CommandResult(CommandError.ObjectNotFound, LogService.CHANNEL_NOT_FOUND);
			} catch (Exception e) {
				LogService.Error(nameof(JoinChannelInteractor), LogService.ERROR, e);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}

		private async Task<string> GetRequestedChannelName(IJoinChannelCommand command, string inputChannelName)
		{
			string selectedChannel;
			if (string.IsNullOrWhiteSpace(inputChannelName))
				selectedChannel = command.GetUserConnectedVoiceChannel();
			else
				selectedChannel = command.GetExistingChannelName(inputChannelName);

			return selectedChannel ?? await SendErrorMessage(command);
		}

		private async Task<string> SendErrorMessage(IJoinChannelCommand command)
		{
			LogService.Warn(nameof(SendErrorMessage), LogService.MISING_CHANNEL_NAME);
			await command.ReplyAsync(LogService.MISING_CHANNEL_NAME);
			return null;
		}

		private async Task ReplyChannelFound(IJoinChannelCommand command, string selectedChannel)
		{
			var message = $"*a rejoint le channel {selectedChannel}.*";
			await command.ReplyAsync(message);
		}

		private async Task<CommandResult> ConnectToChannel(IJoinChannelCommand command, string selectedChannel)
		{

			var audio = await command.ConnectToChannelAsync(selectedChannel);
			audio.Disconnected += OnAudioDisconnected;
			return new CommandResult(null, LogService.SUCCESS);
		}

		private Task OnAudioDisconnected(Exception e)
		{
			// auto reconnect ?
			LogService.Error(nameof(OnAudioDisconnected), LogService.ERROR, e);
			return Task.CompletedTask;
		}
	}
}
