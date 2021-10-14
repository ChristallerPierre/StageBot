using System.Threading.Tasks;

namespace Presentation.Startup.Interface
{
	public interface IBotStartup
	{
		Task StartDiscordHandler();
	}
}