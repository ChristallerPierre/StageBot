using Discord.Commands;
using Newtonsoft.Json;
using StageBot.Controller.PlanningModule;
using StageBot.Presentation;
using StageBot.Interactor.Services;
using StageBot.Modules;
using StageBot.Services;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StageBot.Interactor
{
	public class TopicPlanningInteractor : ITopicPlanningInteractor
	{
		private const string ACTION_LIST = "list";
		private const string ACTION_ADD = "+";
		private const string ACTION_REMOVE = "-";
		private const string ANSWER_UNKNOWN_ACTION = "Action non-reconnue.";
		private const string ANSWER_NO_PLANNING = "Aucune modification de titre plannifiée.";
		private const string ANSWER_PLANNINGS = "Modifications de titres plannifiées :";
		private const string ANSWER_ERR_MODIFICATION_PLANNIFIED = "Erreur : modification de titre déjà plannifiée à cet horaire.";
		private const string ANSWER_ERR_FORMAT_DATE = "Erreur : le format de la date doit être DD/MM HH:mm.";
		private const string ANSWER_PLANNING_ADDED = "Modification de titre ajoutée.";
		private const string ANSWER_PLANNING_REMOVED = "Modification de titre enlevée.";
		private const string ANSWER_ERR_NO_MATCH = "Aucune modification de titre prévue à cette date.";
		private const string FILEPATH = "planning.json";

		private readonly IFileSystem _fileSystem;

		public TopicPlanningInteractor(IFileSystem fileSystem)
		{
			_fileSystem = fileSystem;
		}

		public async Task<RuntimeResult> DispatchPlanningAsync(ITopicPlanningCommand command, string action, string date = null, string hour = null, string title = null)
		{
			try {
				switch (action) {
					case ACTION_LIST:
						return await ListPlannedTitles(command);
					case ACTION_ADD:
						return await AddPlannedTitle(command, date, hour, title);
					case ACTION_REMOVE:
						return await RemovePlannedTitle(command, date, hour);
					default:
						LogService.Warn(nameof(DispatchPlanningAsync), ANSWER_UNKNOWN_ACTION);
						return new CommandResult(CommandError.Unsuccessful, ANSWER_UNKNOWN_ACTION);
				}
			} catch (Exception ex) {
				LogService.Error(nameof(DispatchPlanningAsync), LogService.ERROR, ex);
				return new CommandResult(CommandError.Exception, LogService.ERROR);
			}
		}

		private async Task<RuntimeResult> ListPlannedTitles(ITopicPlanningCommand command)
		{
			if (CreateFileIfMissing()) {
				await command.ReplyAsync(ANSWER_NO_PLANNING);
				return new CommandResult(null, ANSWER_NO_PLANNING);
			} else {
				var plannings = await GetFileContent();
				StringBuilder stringBuilder = new StringBuilder(ANSWER_PLANNINGS);
				foreach (var planning in plannings) {
					stringBuilder.Append($"{Environment.NewLine}- {planning.Date} {planning.Hour} : {planning.Title}");
				}
				var answer = stringBuilder.ToString();
				await command.ReplyAsync(answer);
				return new CommandResult(null, answer);
			}
		}

		private async Task<TopicPlanning[]> GetFileContent()
		{
			var fileContent = await _fileSystem.File.ReadAllTextAsync(FILEPATH);
			TopicPlanning[] plannings = JsonConvert.DeserializeObject<TopicPlanning[]>(fileContent);
			return plannings;
		}

		private async Task<RuntimeResult> AddPlannedTitle(ITopicPlanningCommand command, string date, string hour, string title)
		{
			TopicPlanning newPlanning;
			try {
				newPlanning = GetPlanningInstance(date, hour, null);
			} catch (Exception ex) {
				await command.ReplyAsync(ANSWER_ERR_FORMAT_DATE);
				return new CommandResult(CommandError.ParseFailed, ANSWER_ERR_FORMAT_DATE);
			}

			CreateFileIfMissing();
			var plannings = await GetFileContent();

			if (plannings.Any(p => p.GetDate() == newPlanning.GetDate())) {
				await command.ReplyAsync(ANSWER_ERR_MODIFICATION_PLANNIFIED);
				return new CommandResult(CommandError.Unsuccessful, ANSWER_ERR_MODIFICATION_PLANNIFIED);
			} else {
				var newPlannings = plannings.ToList();
				newPlannings.Add(newPlanning);
				await OverWriteFile(newPlannings);
				await command.ReplyAsync(ANSWER_PLANNING_ADDED);
				return new CommandResult(null, ANSWER_PLANNING_ADDED);
			}
		}

		private async Task<RuntimeResult> RemovePlannedTitle(ITopicPlanningCommand command, string date, string hour)
		{
			TopicPlanning newPlanning;
			try {
				newPlanning = GetPlanningInstance(date, hour, null);
			} catch (Exception ex) {
				await command.ReplyAsync(ANSWER_ERR_FORMAT_DATE);
				return new CommandResult(CommandError.ParseFailed, ANSWER_ERR_FORMAT_DATE);
			}

			CreateFileIfMissing();
			var plannings = await GetFileContent();
			if (!plannings.Any()) {
				await command.ReplyAsync(ANSWER_NO_PLANNING);
				return new CommandResult(CommandError.Unsuccessful, ANSWER_NO_PLANNING);
			}

			var newPlannings = plannings.ToList();
			var nbRemoved = newPlannings.RemoveAll(p => p.GetDate() == newPlanning.GetDate());
			if (nbRemoved == 1) {
				await OverWriteFile(plannings);
				await command.ReplyAsync(ANSWER_PLANNING_REMOVED);
				return new CommandResult(null, ANSWER_PLANNING_REMOVED);
			} else {
				await command.ReplyAsync(ANSWER_ERR_NO_MATCH);
				return new CommandResult(CommandError.Unsuccessful, ANSWER_ERR_NO_MATCH);
			}
		}

		private TopicPlanning GetPlanningInstance(string date, string hour, string title)
		{
			TopicPlanning newPlanning;
			newPlanning = new TopicPlanning() {
				Date = date,
				Hour = hour,
				Title = TopicFormatter.ReformatTopic(title)
			};
			return newPlanning;
		}

		private async Task OverWriteFile(IEnumerable<TopicPlanning> plannings)
		{
			string json = JsonConvert.SerializeObject(plannings, Formatting.Indented);
			await _fileSystem.File.WriteAllTextAsync(FILEPATH, json);
		}

		private bool CreateFileIfMissing()
		{
			if (!_fileSystem.File.Exists(FILEPATH)) {
				_fileSystem.File.Create(FILEPATH);
				return true;
			}
			return false;
		}
	}
}
