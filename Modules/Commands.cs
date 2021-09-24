using System;
using System.Collections.Generic;
using System.Text;

namespace StageBot.Modules
{
	public class Commands
	{
		/// <summary>
		/// Démarre le stage channel
		/// </summary>
		public const string START = "start";
		/// <summary>
		/// Arrête le stage channel
		/// </summary>
		public const string STOP = "stop";

		/// <summary>
		/// Quitte une scène ou un channel vocal
		/// </summary>
		public const string EXIT = "exit";

		/// <summary>
		/// Modifie le titre d'un stage channel
		/// </summary>
		public const string EDIT = "edit";
		public const string TITLE = "title";

		/// <summary>
		/// Rejoindre une scène
		/// </summary>
		public const string STAGE = "stage";
		public const string SCENE = "scene";
		
		/// <summary>
		/// Rejoindre un channel vocal
		/// </summary>
		public const string JOIN = "join";
		
		/// <summary>
		/// Afficher l'aide
		/// </summary>
		public const string HELP = "help";
		public const string QUESTION_MARK = "?";
	}
}
