using Discord.Commands;
using Discord.WebSocket;
using Presentation.Helper;
using System;
using System.Threading.Tasks;

namespace Presentation.Controller.Attribute
{
	public class RequireChannelAttribute : PreconditionAttribute
	{
		private readonly string _name;

		public RequireChannelAttribute(string name)
		{
			_name = name;
		}

		public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
		{
			try {
				if (context.User is SocketGuildUser user) {
					if (context.Channel.Name == _name)
						return Task.FromResult(PreconditionResult.FromSuccess());
					else
						return Task.FromResult(PreconditionResult.FromError($"{UserHelper.GetUserTag(context)} tried to execute the command {context.Message.Content} in channel {context.Channel.Name }"));
				}
				return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command."));
			} catch (Exception ex) {
				return Task.FromResult(PreconditionResult.FromError(ex));
			}
		}
	}
}
