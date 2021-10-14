using Discord;
using System.Threading.Tasks;

namespace Presentation.Controller.Interface
{
	public interface ITopicPlanningCommand
	{
		Task<IUserMessage> ReplyAsync(string text);
	}
}
