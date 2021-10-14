using Discord;
using System.Threading.Tasks;

namespace Presentation.Controller.Interface
{
	public interface IJoinChannelCommand
	{
		Task<IUserMessage> ReplyAsync(string text);
	}
}
