using Discord;
using System.Threading.Tasks;

namespace Presentation.Controller.Interface
{
	public interface IEditStageCommand
	{
		Task<IUserMessage> ReplyAsync(string text);
	}
}
