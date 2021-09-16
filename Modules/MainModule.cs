using Discord;
using Discord.Commands;
using StageBot.Services;
using System.Linq;
using System.Threading.Tasks;

namespace StageBot.Modules
{
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
		public async Task Join(string channelName)
		{
			if (string.IsNullOrWhiteSpace(channelName)) {
				var errorMessage = $"Veuillez préciser le nom d'un channel vocal après la commande.";
				await LoggingService.Log(new LogMessage(LogSeverity.Warning, nameof(Join), errorMessage));

				await ReplyAsync(errorMessage);
				return;
			}

			var channel = Context.Guild.VoiceChannels.SingleOrDefault(channel => channel.Name == channelName);
			if (channel is null) {
				var errorMessage = $"Channel {channelName} non-trouvé.";
				await LoggingService.Log(new LogMessage(LogSeverity.Warning, nameof(Join), errorMessage));
				await ReplyAsync(errorMessage);
				return;
			}

			await channel.ConnectAsync();
			var message = $"*a rejoint le channel {channelName}.*";
			await LoggingService.Log(new LogMessage(LogSeverity.Info, nameof(Join), message));
			await ReplyAsync(message);
		}

		[Command(Commands.HELP)]
		[Alias(Commands.QUESTION_MARK)]
		[Summary("Affiche l'aide")]
		public async Task Help()
		{
			var message = "aide : to be redacted";
			await LoggingService.Log(new LogMessage(LogSeverity.Info, nameof(Join), message));
			await ReplyAsync(message);
		}
	}
}
