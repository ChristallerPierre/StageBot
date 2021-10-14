using Presentation.Controller.Handler;
using Presentation.Controller.Interface;
using System.Threading.Tasks;

namespace Presentation.Interactor.Interface
{
	public interface IJoinChannelInteractor
	{
		Task<CommandResult> JoinAsync(IJoinChannelCommand command, string inputChannelName = "");
	}
}
