using Discord;
using System.Threading.Tasks;

namespace Presentation.Controller.Interface
{
	public interface IHelpCommand
	{
		Task<IUserMessage> ReplyAsync(string text);
	}
}