using Discord.WebSocket;
using Infrastructure.Services;
using Presentation.Controller.Handler;
using Presentation.Controller.Interface;
using Presentation.Interactor.Interface;
using System.Threading.Tasks;

namespace Presentation.Interactor
{
	public class ExitStageInteractor : IExitStageInteractor
	{
		public async Task<CommandResult> Disconnect(IExitStageCommand command, SocketStageChannel stageChannel)
		{
			// todo : unregister events
			await stageChannel.DisconnectAsync();

			var message = $"*a quitté la scène {stageChannel.Name}.*";
			await command.ReplyAsync(message);
			return new CommandResult(null, LogService.SUCCESS);
		}
	}
}
