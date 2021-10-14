using Presentation.Controller.Handler;
using Presentation.Controller.Interface;
using System.Threading.Tasks;

namespace Presentation.Interactor.Interface
{
	public interface IHelpInteractor
	{
		Task<CommandResult> DisplayHelpAsync(IHelpCommand command);
	}
}