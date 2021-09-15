using Discord;
using Discord.Commands;
using StageBot.Setup;
using System.Linq;
using System.Threading.Tasks;

namespace StageBot.Modules
{
	public class MainModule : ModuleBase<SocketCommandContext>
	{
		private class Commands
		{
			public const string JOIN = "join";
		}

		[Command(Commands.JOIN)]
		[Summary("Demande au bot de rejoindre le channel vocal")]
		public async Task Join(string channelName)
		{
			if (string.IsNullOrWhiteSpace(channelName)) {
				var errorMessage = $"Veuillez faire suivre la commande avec le nom d'un channel vocal.";
				await BotStartup.Log(new LogMessage(LogSeverity.Warning, nameof(Join), errorMessage));

				await ReplyAsync(errorMessage);
				return;
			}

			var channel = Context.Guild.VoiceChannels.SingleOrDefault(channel => channel.Name == channelName);
			if (channel is null) {
				var errorMessage = $"Channel {channelName} non-trouvé.";
				await BotStartup.Log(new LogMessage(LogSeverity.Warning, nameof(Join), errorMessage));
				await ReplyAsync(errorMessage);
				return;
			}

			var message = $"*a rejoint le channel {channelName}*";
			await BotStartup.Log(new LogMessage(LogSeverity.Info, nameof(Join), message));
			await ReplyAsync(message);
			await channel.ConnectAsync();
		}
	}
}
