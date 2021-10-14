using Discord.WebSocket;
using Presentation.Controller.Handler;
using Presentation.Controller.Interface;
using System.Threading.Tasks;

namespace Presentation.Interactor.Interface
{
	public interface IExitStageInteractor
	{
		Task<CommandResult> Disconnect(IExitStageCommand command, SocketStageChannel stageChannel);
	}
}
