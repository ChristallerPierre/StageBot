using System;
using System.Threading.Tasks;

namespace StageBot.Setup
{
	public interface IBotStartup
	{
		Task StartDiscordHandler();
	}
}