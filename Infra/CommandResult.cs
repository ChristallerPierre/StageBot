using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace StageBot.Modules
{
	public class CommandResult : RuntimeResult
	{
		public CommandResult(CommandError? error, LogMessage log) : base(error, log.Message)
		{ }
	}
}
