using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Presentation.Controller.Interface
{
	public class BaseCommand : ModuleBase<SocketCommandContext>
	{
		public async Task<IUserMessage> ReplyAsync(string text)
		{
			var userMessage = await base.ReplyAsync(text);
			return userMessage;
		}
	}
}
