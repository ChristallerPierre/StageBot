using Discord;
using System.Threading.Tasks;

namespace Presentation.Controller.Interface
{
	public interface IIJoinChannelCommand
	{
		Task<IUserMessage> ReplyAsync(string text);
	}
}
