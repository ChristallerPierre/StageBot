using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

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
