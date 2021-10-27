using Discord.Commands;
using Discord.WebSocket;
using Infrastructure.Services;
using Presentation.Helper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controller.Attribute
{
	public class RequireUserRoleAttribute : PreconditionAttribute
	{
		private readonly string[] _names;

		public RequireUserRoleAttribute(string name) => _names = new[] { name };

		public RequireUserRoleAttribute(string[] names) => _names = names;

		public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
		{
			try {
				if (context.User is SocketGuildUser user) {
					if (user.Roles.Any(r => _names.Contains(r.Name)))
						return Task.FromResult(PreconditionResult.FromSuccess());
					return Task.FromResult(PreconditionResult.FromError($"{UserHelper.GetUserTag(context)} tried to execute the command {context.Message.Content} without the role {_names}"));
				}
				return Task.FromResult(PreconditionResult.FromError("You must be in a guild to run this command."));
			} catch (Exception ex) {
				LogService.Error(nameof(RequireUserRoleAttribute), LogService.ERROR, ex);
				return Task.FromResult(PreconditionResult.FromError(ex));
			}
		}
	}
}
