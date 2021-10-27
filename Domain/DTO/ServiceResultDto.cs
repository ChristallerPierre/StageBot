using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
	public class ServiceResultDto
	{
		public string Message { get; }
		public ServiceResult Result { get; }

		public ServiceResultDto(string message, ServiceResult result)
		{
			Message = message;
			Result = result;
		}

		public enum ServiceResult
		{
			//
			// Summary:
			//     Thrown when the command is unknown.
			UnknownCommand = 1,
			//
			// Summary:
			//     Thrown when the command fails to be parsed.
			ParseFailed,
			//
			// Summary:
			//     Thrown when the input text has too few or too many arguments.
			BadArgCount,
			//
			// Summary:
			//     Thrown when the object cannot be found by the Discord.Commands.TypeReader.
			ObjectNotFound,
			//
			// Summary:
			//     Thrown when more than one object is matched by Discord.Commands.TypeReader.
			MultipleMatches,
			//
			// Summary:
			//     Thrown when the command fails to meet a Discord.Commands.PreconditionAttribute's
			//     conditions.
			UnmetPrecondition,
			//
			// Summary:
			//     Thrown when an exception occurs mid-command execution.
			Exception,
			//
			// Summary:
			//     Thrown when the command is not successfully executed on runtime.
			Unsuccessful
		}
	}
}
