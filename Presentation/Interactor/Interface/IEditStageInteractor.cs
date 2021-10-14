using Discord.WebSocket;
using Presentation.Controller.Handler;
using Presentation.Controller.Interface;
using System.Threading.Tasks;

namespace Presentation.Interactor.Interface
{
	public interface IEditStageInteractor
	{
		Task<CommandResult> EditTopicAsync(IEditStageCommand command, string topic, SocketStageChannel stageChannel);
	}
}
