using Discord;
using Discord.Commands;
using StageBot.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StageBot.Modules
{
	public class JoinChannelCommand : ModuleBase<SocketCommandContext>
	{
		[Command(CommandList.JOIN, RunMode = RunMode.Async)]
		[Summary("Demande au bot de rejoindre le channel vocal")]
		[Name(CommandList.JOIN)]
		public async Task<RuntimeResult> Join()
		{
			return await ExecuteCommand();
		}

		[Command(CommandList.JOIN, RunMode = RunMode.Async)]
		[Summary("Demande au bot de rejoindre le channel vocal")]
		[Name(CommandList.JOIN)]
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
				return new CommandResult(
					CommandError.ObjectNotFound,
					new LogMessage(
						LogSeverity.Info,
						nameof(JoinChannelCommand),
						LogService.CHANNEL_NOT_FOUND));
			} catch (Exception e) {
				var logMessage = new LogMessage(LogSeverity.Error, nameof(JoinChannelCommand), LogService.ERROR, e);
				await LogService.Log(logMessage);
				return new CommandResult(CommandError.Exception, logMessage);
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
			var errorMessage = $"Veuillez exécuter la commande en étant connecté à un channel vocal, ou préciser le nom d'un channel après la commande.";
			await LogService.Log(new LogMessage(LogSeverity.Warning, nameof(Join), errorMessage));
			await ReplyAsync(errorMessage);
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
			return new CommandResult(
				null,
				new LogMessage(
					LogSeverity.Info,
					nameof(JoinChannelCommand),
					LogService.SUCCESS));
		}

		private async Task OnAudioDisconnected(Exception e)
		{
			await LogService.Log(new LogMessage(LogSeverity.Error, nameof(OnAudioDisconnected), LogService.ERROR, e));
		}
	}
}
