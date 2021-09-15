using System.Threading.Tasks;

namespace StageBot
{
	public interface IBotStartup
	{
		Task MainAsync();
	}
}