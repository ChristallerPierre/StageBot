using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
	public static class TopicFormatterService
	{
		public static string ReformatTopic(string paramInputTopic)
		{
			var inputTopic = paramInputTopic.Trim();
			if ((inputTopic.StartsWith('\'') && inputTopic.EndsWith('\''))
				|| (inputTopic.StartsWith('"') && inputTopic.EndsWith('"'))) {
				inputTopic = inputTopic.Remove(0, 1);
				inputTopic = inputTopic.Remove(inputTopic.Length - 1, 1);
			}
			return inputTopic;
		}
	}
}
