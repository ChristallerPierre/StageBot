using System.Threading.Tasks;

namespace StageBot.Infra
{
	public interface IDiscordClientHandler
	{
		void Dispose();
		Task<bool> Connect();
	}
}
