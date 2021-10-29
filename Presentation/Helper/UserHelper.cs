using Discord.Commands;

namespace Presentation.Helper
{
	public static class UserHelper
	{
		public static string GetUserTag(ICommandContext context)
		{
			return context.User.Username + "#" + context.User.Discriminator;
		}
	}
}
