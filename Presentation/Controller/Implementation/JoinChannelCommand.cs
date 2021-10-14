using Discord;
using Discord.Audio;
using Discord.Commands;
using Presentation.Configuration;
using Presentation.Controller.Attribute;
using Presentation.Controller.Interface;
using Presentation.Interactor.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controller.Implementation
{
	public class JoinChannelCommand : BaseCommand, IJoinChannelCommand
	{
		public const string JOIN = "join";
		public const string CMD_DESC = "!join <nom du channel> pour que le bot rejoigne le channel vocal précisé.";

		IJoinChannelInteractor _interactor;

		public JoinChannelCommand(IJoinChannelInteractor interactor)
		{
			_interactor = interactor;
		}

		[Name(JOIN)]
		[Command(JOIN, RunMode = RunMode.Async)]
		[RequireUserRole(GuildRoles.WIP)]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Join()
		{
			return await _interactor.JoinAsync(this);
		}

		[Name(JOIN)]
		[Command(JOIN, RunMode = RunMode.Async)]
		[RequireUserRole(GuildRoles.WIP)]
		[Summary(CMD_DESC)]
		public async Task<RuntimeResult> Join(string inputChannelName)
		{
			return await _interactor.JoinAsync(this, inputChannelName);
		}


		public string GetUserConnectedVoiceChannel()
		{
			var user = Context.User as IGuildUser;
			return user.VoiceChannel?.Name;
		}

		public string GetExistingChannelName(string inputChannelName)
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

		public async Task<IAudioClient> ConnectToChannelAsync(string selectedChannel)
		{
			var audio = await Context.Guild.VoiceChannels
				.First(chan => chan.Name == selectedChannel)
				.ConnectAsync();
			return audio;
		}
	}
}
