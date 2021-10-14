using System.Threading.Tasks;

namespace StageBot.Infrastructure
{
	public interface IDiscordClientHandler
	{
		void Dispose();
		Task<bool> Connect();
	}
}
