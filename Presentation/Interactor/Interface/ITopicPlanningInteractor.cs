using Discord.Commands;
using Presentation.Controller.Interface;
using System.Threading.Tasks;

namespace Presentation.Interactor.Interface
{
	public interface ITopicPlanningInteractor
	{
		Task<RuntimeResult> DispatchPlanningAsync(ITopicPlanningCommand command,string action, string date = null, string hour = null, string title = null);
	}
}
