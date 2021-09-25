using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace StageBot.Modules
{
	public class CommandResult : RuntimeResult
	{
		public LogMessage Log { get; set; }

		public const string SUCCESS = "Success";
		public const string ERROR = "Error";
		public const string CHANNEL_NOT_FOUND = "Channel not found";

		public CommandResult(CommandError? error, string reason) : base(error, reason)
		{ }

		// todo : use this ctr exclusively
		public CommandResult(CommandError? error, LogMessage log) : base(error, log.Message)
		{
			Log = log;
		}
	}
}
