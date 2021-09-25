using Discord;
using System.Threading.Tasks;

namespace StageBot.Controller.HelpModule
{
	public interface IHelpCommand
	{
		Task<IUserMessage> ReplyAsync(string text);
	}
}