using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.API;
using System.Text;
using System.Linq;

static class Language {
	private static Dictionary<string, Dictionary<string, string>> langData = new Dictionary<string, Dictionary<string, string>> {
		{ "german", new Dictionary<string, string> {
			{ "wrong_password", "Falsches Passwort!" },
			{ "account_doesnt_exist", "Account existiert nicht!" },
			{ "too_long_outside_map", "Du warst zu lange außerhalb der Map!" },
			{ "adminlvl_not_high_enough", "Dein Adminlevel ist nicht hoch genug dafür!" },
			{ "unban", "{1} wurde von {2} entbannt. Grund: {3}" },
			{ "permaban", "{1} wurde permanent von {2} gebannt. Grund: {3}" },
			{ "timeban", "{1} wurde für {2} Stunden von {3} gebannt. Grund: {4}" },
			{ "youpermaban", "Du wurdest permanent von {1} gebannt. Grund: {2}" },
			{ "youtimeban", "Du wurdest für {1} Stunden von {2} gebannt. Grund: {3}" },
			{ "kick", "{1} wurde von {2} gekickt. Grund: {3}" },
			{ "youkick", "Du wurdest von {1} gekickt. Grund: {2}" },
			{ "lobbykick", "{1} wurde von {2} aus der Lobby gekickt. Grund: {3}" },
			{ "youlobbykick",  "Du wurdest von {1} aus der Lobby gekickt. Grund: {2}" },
			{ "connecting", "Verbindet ..." },
		} },
		{ "english", new Dictionary<string, string> {
			{ "wrong_password", "Wrong password!" },
			{ "account_doesnt_exist", "Account doesn't exist!" },
			{ "too_long_outside_map", "You've been too long outside the map!" },
			{ "adminlvl_not_high_enough", "Your adminlevel isn't high enough for this action!" },
			{ "unban", "{1} was unbaned by {2}. Reason: {3}" },
			{ "permaban", "{1} was baned permanently by {2}. Reason: {3}" },
			{ "timeban", "{1} was baned for {2} hours by {3}. Reason: {4}" },
			{ "youpermaban", "You were baned permanently by {1}. Reason: {2}" },
			{ "youtimeban", "You were baned for {1} hours by {2}. Reason: {3}" },
			{ "kick", "{1} was kicked by {2}. Reason: {3}" },
			{ "youkick", "You were kicked by {1}. Reason: {2}" },
			{ "lobbykick", "{1} was kicked by {2} out of the lobby. Reason: {3}" },
			{ "youlobbykick", "You were kicked by {1} out of the lobby. Reason: {2}" },
			{ "connecting", "connecting ..." },
		} }
	};

	public static string GetLang ( this Client player, string type, string arg1 = "", string arg2 = "", string arg3 = "", string arg4 = "" ) {
		string language = player.GetChar ().language;
		if ( arg1 == "" )
			return langData[language][type];
		else
			return GetReplaced ( langData[language][type], arg1, arg2, arg3, arg4 );
	}

	public static void SendLangMessage ( this Client player, string type ) {
		player.sendChatMessage ( player.GetLang ( type ) );
	}

	public static void SendLangNotification ( this Client player, string type ) {
		API.shared.sendNotificationToPlayer ( player, player.GetLang ( type ) );
	}

	public static string GetReplaced ( string arg, string arg1, string arg2 = "", string arg3 = "", string arg4 = "" ) {
		StringBuilder builder = new StringBuilder ( arg );
		builder.Replace ( "{1}", arg1 );
		if ( arg2 != "" ) {
			builder.Replace ( "{2}", arg2 );
		}
		if ( arg3 != "" ) {
			builder.Replace ( "{3}", arg3 );
		}
		if ( arg4 != "" ) {
			builder.Replace ( "{4}", arg4 );
		}
		return builder.ToString ();
	}

	public static void SendMessageToAll ( string type, string arg1 = "", string arg2 = "", string arg3 = "", string arg4 = "" ) {
		Dictionary<string, string> texts = new Dictionary<string, string> ();
		List<string> keys = langData.Keys.ToList ();
		for ( int i = 0; i < keys.Count; i++ ) {
			string lang = keys[i];
			if ( arg1 == "" )
				texts[lang] = langData[lang][type];
			else
				texts[lang] = GetReplaced ( langData[lang][type], arg1, arg2, arg3, arg4 );
		}
		List<Client> players = API.shared.getAllPlayers ();
		for ( int i = 0; i < players.Count; i++ ) {
			players[i].sendChatMessage ( texts[players[i].GetChar ().language] );
		}
	}

	public static void SendNotificationToAll ( string type, string arg1 = "", string arg2 = "", string arg3 = "", string arg4 = "" ) {
		Dictionary<string, string> texts = new Dictionary<string, string> ();
		List<string> keys = langData.Keys.ToList ();
		for ( int i = 0; i < keys.Count; i++ ) {
			string lang = keys[i];
			if ( arg1 == "" )
				texts[lang] = langData[lang][type];
			else
				texts[lang] = GetReplaced ( langData[lang][type], arg1, arg2, arg3, arg4 );
		}
		List<Client> players = API.shared.getAllPlayers ();
		for ( int i = 0; i < players.Count; i++ ) {
			API.shared.sendNotificationToPlayer ( players[i], texts[players[i].GetChar ().language] );
		}
	}
}
