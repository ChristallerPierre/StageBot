﻿using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace StageBot.Infra
{
	public static class UserHelper
	{
		public static string GetUserTag(ICommandContext context)
		{
			return context.User.Username + "#" + context.User.Discriminator;
		}

		internal static object GetUserTag(object conte) => throw new NotImplementedException();
	}
}