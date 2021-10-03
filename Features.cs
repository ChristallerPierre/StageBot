namespace StageBot
{
	class Features
	{
		/*
		 * TODO - Nouvelles fonctionnalités :
		 *	- planification des changements de titres -> lister, ajouter et supprimer une planification
		 *	- pouvoir lister les titres précédents
		 *	- synthèse vocal pour message à diffuser (message, delai entre deux diffusions)
		 *	- messages d'erreur quand commande executée sans paramètre / en cas d'erreur d'exec
		 *	- /commands
		 *	- [RequireBotPermission] pour vérifier que le bot à les droits avant d'exécuter une commande
		 *	- start with ongoing scene -> edit title
		 *	- !start <input> when not connected -> connect then start
		 *	- !edit when not connected
		 *	- !edit when stage not started
		 *	- utiliser ce bot pour envoyer des réponses pré-programmées
		 * 
		 * TODO - Bugfixes :
		 *	 - bugs listed on various parts of the app
		 *	 - fix time-out sur connexion audio
		 *	 - ne pas log les OpCode quand connecté à un channel vocal
		 *	 - join -> forbidden
		 *	 - crash during night, before 3/10 14h45
		 *	 - on !exit : System.Threading.Tasks.TaskCanceledException: A task was canceled.
		 *			at Discord.ConnectionManager.<>c__DisplayClass29_0.<<StartAsync>b__0>d.MoveNext()
		 */
	}
}
