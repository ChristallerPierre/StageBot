using Discord;
using Discord.Commands;
using StageBot.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StageBot.Modules
{
	// todo : fix time-out sur connexion audio
	// todo : ajouter gestion des rôles

	public class JoinModule : ModuleBase<SocketCommandContext>
	{
		[Command(Commands.JOIN)]
		[Summary("Demande au bot de rejoindre le channel vocal")]
		[Name(Commands.JOIN)]
		public async Task Join(string inputChannelName)
		{
			try {
				if (await HandleInputEmptyAsync(inputChannelName))
					return;

				var selectedChannel = GetSelectedChannelName(inputChannelName);

				if (await HandleChannelNotFoundAsync(selectedChannel, inputChannelName))
					return;

				await HandleChannelFoundAsync(selectedChannel);
			} catch (Exception e) {
				await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(Join), "Error", e));
			}
		}

		private async Task<bool> HandleInputEmptyAsync(string inputChannelName)
		{
			if (string.IsNullOrWhiteSpace(inputChannelName)) {
				var errorMessage = $"Veuillez préciser le nom d'un channel vocal après la commande.";
				await LoggingService.Log(new LogMessage(LogSeverity.Warning, nameof(Join), errorMessage));
				await ReplyAsync(errorMessage);
				return true;
			}
			return false;
		}

		private string GetSelectedChannelName(string inputChannelName)
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
				.chan;
			return selectedChannel;
		}

		private async Task<bool> HandleChannelNotFoundAsync(string selectedChannel, string inputChannelName)
		{
			if (selectedChannel is null) {
				var errorMessage = $"Channel {inputChannelName} non-trouvé.";
				await LoggingService.Log(new LogMessage(LogSeverity.Warning, nameof(Join), errorMessage));
				await ReplyAsync(errorMessage);
				return true;
			}
			return false;
		}

		private async Task HandleChannelFoundAsync(string selectedChannel)
		{
			var message = $"*a rejoint le channel {selectedChannel}.*";
			//await LoggingService.Log(new LogMessage(LogSeverity.Info, nameof(Join), message));
			await ReplyAsync(message);
			var audio = await Context.Guild.VoiceChannels
				.First(chan => chan.Name == selectedChannel)
				.ConnectAsync();
			audio.Disconnected += OnAudioDisconnected;
		}

		private async Task OnAudioDisconnected(Exception e)
		{
			await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(OnAudioDisconnected), "Error", e));
		}
	}
}
