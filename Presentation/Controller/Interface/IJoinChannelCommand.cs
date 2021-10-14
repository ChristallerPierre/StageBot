using Discord;
using Discord.Audio;
using System.Threading.Tasks;

namespace Presentation.Controller.Interface
{
	public interface IJoinChannelCommand
	{
		Task<IUserMessage> ReplyAsync(string text);
		string GetExistingChannelName(string inputChannelName);
		string GetUserConnectedVoiceChannel();
		Task<IAudioClient> ConnectToChannelAsync(string selectedChannel);
	}
}
