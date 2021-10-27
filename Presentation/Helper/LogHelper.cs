using Discord.Commands;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Helper
{
	public static class LogHelper
	{
		public static string ReadCommandContext(
			ICommandContext context,
			string commandName,
			IResult result = null)
		{
			return LogService.ReadCommandContext(
				context.Channel.Name,
				UserHelper.GetUserTag(context),
				context.Message.Content,
				commandName,
				result);
		}
	}
}
