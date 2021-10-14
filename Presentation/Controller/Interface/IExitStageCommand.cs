using Discord;
using System.Threading.Tasks;

namespace Presentation.Controller.Interface
{
	public interface IExitStageCommand
	{
		Task<IUserMessage> ReplyAsync(string text);
	}
}
