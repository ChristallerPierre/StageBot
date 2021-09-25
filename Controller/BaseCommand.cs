using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Controller
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
