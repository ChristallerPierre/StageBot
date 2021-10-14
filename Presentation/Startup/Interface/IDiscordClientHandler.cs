using System.Threading.Tasks;

namespace Presentation.Startup.Interface
{
	public interface IDiscordClientHandler
	{
		void Dispose();
		Task<bool> Connect();
	}
}
