using Discord.Commands;

namespace Presentation.Controller.Handler
{
	public class CommandResult : RuntimeResult
	{
		public CommandResult(CommandError? error, string message) : base(error, message)
		{ }
	}
}
