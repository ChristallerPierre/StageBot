using Discord.Commands;
using Discord.WebSocket;
using Domain.Services;
using Infrastructure.Services;
using Presentation.Controller.Handler;
using Presentation.Controller.Implementation;
using Presentation.Controller.Interface;
using Presentation.Interactor.Interface;
using System;
using System.Threading.Tasks;

namespace Presentation.Interactor
{
	public class EditStageInteractor : IEditStageInteractor
	{
		public async Task<CommandResult> EditTopicAsync(IEditStageCommand command, string topic, SocketStageChannel stageChannel)
		{
			try {
				topic = TopicFormatterService.ReformatTopic(topic);

				await stageChannel.ModifyInstanceAsync(prop => prop.Topic = topic);

				var message = $"*a changé le sujet de la scène {stageChannel.Name} en {topic}.*";
				await command.ReplyAsync(message);

				return new CommandResult(null, LogService.TOPIC_UPDATED);
			} catch (Exception ex) {
				LogService.Error(nameof(EditStageCommand), LogService.ERROR, ex);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}
	}
}
