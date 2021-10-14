using Discord;
using Discord.Commands;
using StageBot.Controller.Precondition;
using StageBot.Infrastructure.Configuration;
using StageBot.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StageBot.Modules
{
	public class JoinChannelCommand : ModuleBase<SocketCommandContext>
	{
		public const string JOIN = "join";
		public const string CMD_DESC = "!join <nom du channel> pour que le bot rejoigne le channel vocal précisé.";

		[Name(JOIN)]
		[Command(JOIN, RunMode = RunMode.Async)]
		[RequireUserRole(GuildRoles.WIP)]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Join()
		{
			return await ExecuteCommand();
		}

		[Name(JOIN)]
		[Command(JOIN, RunMode = RunMode.Async)]
		[RequireUserRole(GuildRoles.WIP)]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Join(string inputChannelName)
		{
			return await ExecuteCommand(inputChannelName);
		}

		private async Task<RuntimeResult> ExecuteCommand(string inputChannelName = "")
		{
			try {
				var selectedChannel = await GetRequestedChannelName(inputChannelName);
				if (selectedChannel != null)
					return await HandleChannelFoundAsync(selectedChannel);
				return new CommandResult(CommandError.ObjectNotFound, LogService.CHANNEL_NOT_FOUND);
			} catch (Exception e) {
				LogService.Error(nameof(JoinChannelCommand), LogService.ERROR, e);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}

		private async Task<string> GetRequestedChannelName(string inputChannelName)
		{
			string selectedChannel;
			if (string.IsNullOrWhiteSpace(inputChannelName))
				selectedChannel = GetUserConnectedVoiceChannel();
			else
				selectedChannel = GetExistingChannelName(inputChannelName);

			return selectedChannel ?? await SendErrorMessage();
		}

		private string GetUserConnectedVoiceChannel()
		{
			var user = Context.User as IGuildUser;
			return user.VoiceChannel?.Name;
		}

		private async Task<string> SendErrorMessage()
		{
			LogService.Warn(nameof(SendErrorMessage), LogService.MISING_CHANNEL_NAME);
			await ReplyAsync(LogService.MISING_CHANNEL_NAME);
			return null;
		}

		private string GetExistingChannelName(string inputChannelName)
		{
			var sanitizedInput = inputChannelName.ToLower().Trim();
			var selectedChannel = Context.Guild.VoiceChannels
				.Select(chan => new {
					chan = chan.Name,
					index = chan.Name.ToLower().IndexOf(sanitizedInput)
				})
				.Where(o => o.index != -1)
				.OrderBy(o => o.index)
				.ThenBy(o => o.chan)
				.FirstOrDefault()
				?.chan;
			return selectedChannel;
		}

		private async Task<RuntimeResult> HandleChannelFoundAsync(string selectedChannel)
		{
			var message = $"*a rejoint le channel {selectedChannel}.*";
			await ReplyAsync(message);
			var audio = await Context.Guild.VoiceChannels
				.First(chan => chan.Name == selectedChannel)
				.ConnectAsync();
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
