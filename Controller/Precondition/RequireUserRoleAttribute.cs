using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Controller.Precondition
{
	public class RequireUserRoleAttribute : PreconditionAttribute
	{
		private readonly string _name;

		public RequireUserRoleAttribute(string name) => _name = name;

		public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
		{
			try {
				if (context.User is SocketGuildUser user) {
					if (user.Roles.Any(r => r.Name == _name))
						return Task.FromResult(PreconditionResult.FromSuccess());
					return Task.FromResult(PreconditionResult.FromError($"{user.Nickname} tried to execute the command {context.Message.Content} without the role {_name}"));
				}
				return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command."));
			} catch (Exception ex) {
				return Task.FromResult(PreconditionResult.FromError(ex));
			}
		}
	}
}
