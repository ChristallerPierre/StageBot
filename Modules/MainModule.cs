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
	// todo : Gateway - A MessageReceived handler is blocking the gateway task. -> force déco ?

	public class MainModule : ModuleBase<SocketCommandContext>
	{
		private class Commands
		{
			public const string JOIN = "join";
			public const string HELP = "help";
			public const string QUESTION_MARK = "?";
		}

		[Command(Commands.JOIN)]
		[Summary("Demande au bot de rejoindre le channel vocal")]
		[Name(Commands.JOIN)]
		public async Task Join(string inputChannelName)
		{
			try {
				if (string.IsNullOrWhiteSpace(inputChannelName)) {
					var errorMessage = $"Veuillez préciser le nom d'un channel vocal après la commande.";
					await LoggingService.Log(new LogMessage(LogSeverity.Warning, nameof(Join), errorMessage));
					await ReplyAsync(errorMessage);
					return;
				}

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

				if (selectedChannel is null) {
					var errorMessage = $"Channel {inputChannelName} non-trouvé.";
					await LoggingService.Log(new LogMessage(LogSeverity.Warning, nameof(Join), errorMessage));
					await ReplyAsync(errorMessage);
					return;
				}

				var message = $"*a rejoint le channel {selectedChannel}.*";
				await LoggingService.Log(new LogMessage(LogSeverity.Info, nameof(Join), message));
				await ReplyAsync(message);
				var audio = await Context.Guild.VoiceChannels
					.First(chan => chan.Name == selectedChannel)
					.ConnectAsync();
				audio.Disconnected += OnAudioDisconnected;
			} catch (Exception e) {
				await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(Join), "Error", e));
			}
		}

		private async Task OnAudioDisconnected(Exception e)
		{
			await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(OnAudioDisconnected), "Error", e));
		}

		[Command(Commands.HELP)]
		[Alias(Commands.QUESTION_MARK)]
		[Summary("Affiche l'aide")]
		[Name(Commands.HELP)]
		public async Task Help()
		{
			var message = "aide : to be redacted";
			await LoggingService.Log(new LogMessage(LogSeverity.Info, nameof(Join), message));
			await ReplyAsync(message);
		}
	}
}
