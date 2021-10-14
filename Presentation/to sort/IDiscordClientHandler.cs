using System.Threading.Tasks;

namespace StageBot.Presentation
{
	public interface IDiscordClientHandler
	{
		void Dispose();
		Task<bool> Connect();
	}
}
