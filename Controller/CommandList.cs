using System;
using System.Collections.Generic;
using System.Text;

namespace StageBot.Modules
{
	public class CommandList
	{
		public static Dictionary<string, string> Commands = new Dictionary<string, string>() {
			{ START, "!start <titre> pour démarrer la présentation avec la scène." },
			{ STOP, "!stop pour arrêter la présentation avec la scène." },
			{ EXIT, "!exit pour faire sortir le bot du channel vocal" },
			{ TITRE, "!titre <nouveau titre> pour changer le titre de la présentation." },
			{ SCENE, "!scene <nom de la scène> pour que le bot rejoigne le channel de la scène précisée." },
			{ JOIN, "!join <nom du channel> pour que le bot rejoigne le channel vocal précisé." },
		};

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
		public const string LEAVE = "leave";

		/// <summary>
		/// Modifie le titre d'un stage channel
		/// </summary>
		public const string EDIT = "edit";
		public const string TITRE = "titre";

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
