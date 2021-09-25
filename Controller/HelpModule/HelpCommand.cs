using Discord;
using Discord.Commands;
using StageBot.Interactor;
using StageBot.Modules;
using System.Threading.Tasks;

namespace StageBot.Controller.HelpModule
{
	public class HelpCommand : ModuleBase<SocketCommandContext>, IHelpCommand
	{
		IHelpInteractor _interactor;

		public HelpCommand(IHelpInteractor interactor)
		{
			_interactor = interactor;
		}

		[Command(CommandList.HELP)]
		[Alias(CommandList.QUESTION_MARK)]
		[Summary("Affiche l'aide")]
		[Name(CommandList.HELP)]
		public async Task<RuntimeResult> Help()
		{
			return await _interactor.DisplayHelpAsync(this);
		}

		public async Task<IUserMessage> ReplyAsync(string text)
		{
			var userMessage = await base.ReplyAsync(text);
			return userMessage;
		}
	}
}
