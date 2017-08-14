using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.API;
using System.Text;
using System.Linq;

static class Language {
	public static List<string> languages = new List<string> { "english", "german" };
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
			{ "got_last_hitted_kill", "Du hast {1} zuletzt getroffen und den Kill bekommen." },
			{ "got_assist", "Du hast den Assist von {1} bekommen." },
			{ "map_won_voting", "Die Map {1} hat das Voting gewonnen!" },
			{ "player_doesnt_exist", "Der Spieler existiert nicht!" },
			{ "not_more_maps_for_voting_allowed", "Es dürfen nur 6 Maps im Voting sein!" },
			{ "commited_suicide", "Du hast Selbstmord begangen." },
			{ "activated_hitsound", "Hitsound aktiviert!" },
			{ "deactivated_hitsound", "Hitsound deaktiviert!" },
			{ "round_reward", "~w~Runden-Belohnung:~n~Kills: ~g~${1}~n~~w~Assists: ~g~${2}~n~~w~Damage: ~g~${3}~n~~o~Insgesamt: ~g~${4}" },

			{ "welcome_1", "Willkommen auf dem ~b~Team Deathmatch Server~w~." },
			{ "welcome_2", "Für Ankündigungen, Support, Bug-Meldung usw." },
			{ "welcome_3", "bitte unseren Discord-Server nutzen:" },
			{ "welcome_4", "discord.gg/ntVnGFt" },
			{ "welcome_5", "Viel Spaß wünscht das ~b~TDS-Team~w~!" },
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
			{ "got_last_hitted_kill", "You hitted {1} last and got the kill." },
			{ "got_assist", "You got the assist of {1}." },
			{ "map_won_voting", "The map {1} won the voting!" },
			{ "player_doesnt_exist", "The player doesn't exist!" },
			{ "not_more_maps_for_voting_allowed", "There can be only 6 maps in voting!" },
			{ "commited_suicide", "You commited suicide." },
			{ "activated_hitsound", "Hitsound activated!" },
			{ "deactivated_hitsound", "Hitsound deactivated!" },
			{ "round_reward", "~w~Round-reward:~n~Kills: ~g~${1}~n~~w~Assists: ~g~${2}~n~~w~Damage: ~g~${3}~n~~o~Total: ~g~${4}" },

			{ "welcome_1", "Welcome to ~b~Team Deathmatch Server~w~." },
			{ "welcome_2", "For announcements, support, bug-reports etc." },
			{ "welcome_3", "please visit our Discord-server:" },
			{ "welcome_4", "discord.gg/ntVnGFt" },
			{ "welcome_5", "Have fun wishes you the ~b~TDS-Team~w~!" },
		} }
	};

	public static string GetLang ( this Client player, string type, params string[] args ) {
		string language = player.GetChar ().language;
		if ( args.Length == 0 )
			return langData[language][type];
		else
			return GetReplaced ( langData[language][type], args );
	}

	public static void SendLangMessage ( this Client player, string type, params string[] args ) {
		player.sendChatMessage ( player.GetLang ( type, args ) );
	}

	public static void SendLangNotification ( this Client player, string type, params string[] args ) {
		API.shared.sendNotificationToPlayer ( player, player.GetLang ( type, args ) );
	}

	public static string GetReplaced ( string str, params string[] args ) {
		StringBuilder builder = new StringBuilder ( str );
		for ( int i = 0; i < args.Length; i++ ) {
			builder.Replace ( "{" + ( i + 1 ) + "}", args[i] );
		}
		return builder.ToString ();
	}

	public static void SendMessageToAll ( string type, params string[] args ) {
		Dictionary<string, string> texts = new Dictionary<string, string> ();
		List<string> keys = langData.Keys.ToList ();
		for ( int i = 0; i < keys.Count; i++ ) {
			string lang = keys[i];
			if ( args.Length == 0 )
				texts[lang] = langData[lang][type];
			else
				texts[lang] = GetReplaced ( langData[lang][type], args );
		}
		List<Client> players = API.shared.getAllPlayers ();
		for ( int i = 0; i < players.Count; i++ ) {
			players[i].sendChatMessage ( texts[players[i].GetChar ().language] );
		}
	}

	public static void SendNotificationToAll ( string type, params string[] args ) {
		Dictionary<string, string> texts = new Dictionary<string, string> ();
		List<string> keys = langData.Keys.ToList ();
		for ( int i = 0; i < keys.Count; i++ ) {
			string lang = keys[i];
			if ( args.Length == 0 )
				texts[lang] = langData[lang][type];
			else
				texts[lang] = GetReplaced ( langData[lang][type], args );
		}
		List<Client> players = API.shared.getAllPlayers ();
		for ( int i = 0; i < players.Count; i++ ) {
			API.shared.sendNotificationToPlayer ( players[i], texts[players[i].GetChar ().language] );
		}
	}
}
