using StageBot.Controller.HelpModule;
using StageBot.Modules;
using System.Threading.Tasks;

namespace StageBot.Interactor
{
	public interface IHelpInteractor
	{
		public Task<CommandResult> DisplayHelpAsync(IHelpCommand command);
	}
}