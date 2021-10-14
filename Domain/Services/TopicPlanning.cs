using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Services
{
	public class TopicPlanning
	{
		public string Date;
		public string Hour;
		public string Title;

		private const string DATE_SPLITTER = "/";
		private const string HOUR_SPLITTER = ":";

		public DateTime GetDate()
		{
			var dateParts = Date.Split(DATE_SPLITTER);
			if (dateParts.Length != 2)
				throw new ArgumentException($"Wrong nb of parts in date {Date}");
			var day = int.Parse(dateParts[0]);
			var month = int.Parse(dateParts[1]);

			var hourParts = Date.Split(HOUR_SPLITTER);
			if (hourParts.Length != 2)
				throw new ArgumentException($"Wrong nb of parts in hour {Hour}");
			var hour = int.Parse(hourParts[0]);
			var minute = int.Parse(hourParts[1]);

			return new DateTime(DateTime.Now.Year, month, day, hour, minute, 0);
		}
	}
}
