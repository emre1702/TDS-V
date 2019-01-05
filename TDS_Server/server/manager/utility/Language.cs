﻿namespace TDS.server.manager.utility
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using extend;
	using GTANetworkAPI;
	using TDS.server.enums;
	using TDS.server.instance.player;

	static class ServerLanguage
	{
		private static readonly Dictionary<Language, Dictionary<string, string>> langData = new Dictionary<Language, Dictionary<string, string>>
		{
			[Language.ENGLISH] = new Dictionary<string, string>
			{
				["wrong_password"] = "Wrong password!",
				["account_doesnt_exist"] = "Account doesn't exist!",
				["too_long_outside_map"] = "You've been too long outside the map!",
				["adminlvl_not_high_enough"] = "Your adminlevel isn't high enough for this action!",
				["unban"] = "{1} was unbaned by {2}. Reason: {3}",
				["permaban"] = "{1} was baned permanently by {2}. Reason: {3}",
				["timeban"] = "{1} was baned for {2} hours by {3}. Reason: {4}",
				["unban_lobby"] = "{1} was unbaned in lobby by {2}. Reason: {3}",
				["permaban_lobby"] = "{1} was baned permanently in lobby by {2}. Reason: {3}",
				["timeban_lobby"] = "{1} was baned for {2} hours in lobby by {3}. Reason: {4}",
				["unbaned_in_lobby"] = "You got unbaned in lobby {1} by {2}. Reason: {3}",
				["still_permabaned_in_lobby"] = "You got permanently baned by {1} in this lobby. Reason: {2}",
				["still_baned_in_lobby"] = "You are still baned until {1} by {2}. Reason: {3}",
				["unmute"] = "{1} was unmuted by {2}. Reason: {3}",
				["permamute"] = "{1} was muted permanently by {2}. Reason: {3}",
				["timemute"] = "{1} was muted for {2} minutes by {3}. Reason: {4}",
				["youpermaban"] = "You were baned permanently by {1}. Reason: {2}",
				["youtimeban"] = "You were baned for {1} hours by {2}. Reason: {3}",
				["kick"] = "{1} was kicked by {2}. Reason: {3}",
				["youkick"] = "You were kicked by {1}. Reason: {2}",
				["lobbykick"] = "{1} was kicked by {2} out of the lobby. Reason: {3}",
				["youlobbykick"] = "You were kicked by {1} out of the lobby. Reason: {2}",
				["connecting"] = "connecting ...",
				["got_last_hitted_kill"] = "You hitted {1} last and got the kill.",
				["got_assist"] = "You got the assist of {1}.",
				["map_won_voting"] = "The map {1} won the voting!",
				["player_doesnt_exist"] = "The player doesn't exist!",
				["not_more_maps_for_voting_allowed"] = "There can be only 6 maps in voting!",
				["commited_suicide"] = "You commited suicide.",
				["activated_hitsound"] = "Hitsound activated!",
				["deactivated_hitsound"] = "Hitsound deactivated!",
				["round_reward"] = "#w#Round-reward:#n#Kills: #g#${1}#n##w#Assists: #g#${2}#n##w#Damage: #g#${3}#n##o#Total: #g#${4}",
				["deathinfo_killed"] = "{1} killed {2} with {3}",
				["deathinfo_died"] = "{1} died",
				["round_mission_normal"] = "Mission: All opponents have to die.",
				["round_mission_bomb_spectator"] = "Mission: Place & defend the bomb or defuse it - or kill all opponents.",
				["round_mission_bomb_good"] = "Mission: Prevent the bomb-explosion or kill all opponents!",
				["round_mission_bomb_bad"] = "Mission: Let the bomb explode on one of the spots or kill all opponents!",
				["bomb_planted"] = "Bomb was planted!",
				["killing_spree_healtharmor"] = "{1} has got a {2}-killingspree and gets a {3} life/armor bonus.",
				["plant_info"] = "To plant the bomb you have to take your fists and hold the left mouse button on one of the bomb-spots.",
				["defuse_info_1"] = "Round-time got changed. Now you have to kill all opponents or defuse the bomb.",
				["defuse_info_2"] = "To defuse the bomb go to the red blip on your minimap (bomb), take your fists and hold the left mouse button.",
				["welcome_1"] = "Welcome to #b#Team Deathmatch Server#w#.",
				["welcome_2"] = "For announcements, support, bug-reports etc.",
				["welcome_3"] = "please visit our Discord-server:",
				["welcome_4"] = "discord.gg/ntVnGFt",
				["welcome_5"] = "You can get/hide the cursor with END.",
				["welcome_6"] = "If you can't move, use ALT+TAB.",
				["welcome_7"] = "Have fun wishes you the #b#TDS-Team#w#!",
				["order_attack"] = "{1}Attack! Go go go!",
				["order_stay_back"] = "{1}Stay back!",
				["order_spread_out"] = "{1}Spread out!",
				["order_go_to_bomb"] = "{1}Go to the bomb!",
				["target_not_logged_in"] = "The target is not logged in.",
				["target_not_in_same_lobby"] = "The target is not in the same lobby.",
				["round_end_death"] = "All opponents are dead!<br>Team {1} wins!",
				["round_end_death_all"] = "All players are dead!<br>No team wins!",
				["round_end_time"] = "Time's up!<br>Team {1} wins because of most HP left!",
				["round_end_bomb_exploded"] = "Bomb exploded!<br>Team {1} wins!",
				["round_end_bomb_defused"] = "Bomb defused!<br>Team {1} wins!",
				["round_end_command"] = "Map was skipped by {1}!",
				["round_end_new_player"] = "Enough players in ...<br>Round starting!",
				["not_allowed"] = "You are not allowed to do that!",
				["gang_doesnt_exist_anymore"] = "The gang doesn't exist anymore!",
				["invitation_was_removed"] = "The invitation already got removed!",
				["created_report"] = "A report with ID {1} was created.",
				["answered_report"] = "The creator answered to his report with ID {1}.",
				["got_report_answer"] = "A team-member answered your report with ID {1}.",
				["reason_missing"] = "The reason is missing!",
				["player_not_muted"] = "The player is not muted!",
				["player_already_muted"] = "The player is already muted!",
				["you_are_still_permamuted"] = "You are still permamuted.",
				["you_are_still_muted"] = "You are still muted for {1} minutes.",
				["not_possible_in_this_lobby"] = "Not possible in this lobby!",
				["you_got_offline_messages_without_new"] = "You got {1} offline messages in userpanel.",
				["you_got_offline_messages_with_new"] = "You got {1} offline messages in userpanel, {2} is/are new.",
				["gang_removed"] = "Your gang got disbanded."
			},
			[Language.GERMAN] = new Dictionary<string, string>
			{
				["wrong_password"] = "Falsches Passwort!",
				["account_doesnt_exist"] = "Account existiert nicht!",
				["too_long_outside_map"] = "Du warst zu lange außerhalb der Map!",
				["adminlvl_not_high_enough"] = "Dein Adminlevel ist nicht hoch genug dafür!",
				["unban"] = "{1} wurde von {2} entbannt. Grund: {3}",
				["permaban"] = "{1} wurde permanent von {2} gebannt. Grund: {3}",
				["timeban"] = "{1} wurde für {2} Stunden von {3} gebannt. Grund: {4}",
				["unban_lobby"] = "{1} wurde in der Lobby von {2} entbannt. Grund: {3}",
				["permaban_lobby"] = "{1} wurde permanent aus der Lobby von {2} gebannt. Grund: {3}",
				["timeban_lobby"] = "{1} wurde für {2} Stunden aus der Lobby von {3} gebannt. Grund: {4}",
				["unbaned_in_lobby"] = "Du wurdest in der Lobby {1} von {2} entbannt. Grund: {3}",
				["still_permabaned_in_lobby"] = "Du wurdest von {1} aus dieser Lobby permanent gebannt. Grund: {2}",
				["still_baned_in_lobby"] = "Du hast noch einen Ban bis {1} von {2}. Grund: {3}",
				["unmute"] = "{1} wurde von {2} entmutet. Grund: {3}",
				["permamute"] = "{1} wurde von {2} permanent gemutet. Grund: {3}",
				["timemute"] = "{1} wurde von {2} für {3} Minuten gemutet. Grund: {4}",
				["youpermaban"] = "Du wurdest permanent von {1} gebannt. Grund: {2}",
				["youtimeban"] = "Du wurdest für {1} Stunden von {2} gebannt. Grund: {3}",
				["kick"] = "{1} wurde von {2} gekickt. Grund: {3}",
				["youkick"] = "Du wurdest von {1} gekickt. Grund: {2}",
				["lobbykick"] = "{1} wurde von {2} aus der Lobby gekickt. Grund: {3}",
				["youlobbykick"] = "Du wurdest von {1} aus der Lobby gekickt. Grund: {2}",
				["connecting"] = "Verbindet ...",
				["got_last_hitted_kill"] = "Du hast {1} zuletzt getroffen und den Kill bekommen.",
				["got_assist"] = "Du hast den Assist von {1} bekommen.",
				["map_won_voting"] = "Die Map {1} hat das Voting gewonnen!",
				["player_doesnt_exist"] = "Der Spieler existiert nicht!",
				["not_more_maps_for_voting_allowed"] = "Es dürfen nur 6 Maps im Voting sein!",
				["commited_suicide"] = "Du hast Selbstmord begangen.",
				["activated_hitsound"] = "Hitsound aktiviert!",
				["deactivated_hitsound"] = "Hitsound deaktiviert!",
				["round_reward"] = "#w#Runden-Belohnung:#n#Kills: #g#${1}#n##w#Assists: #g#${2}#n##w#Damage: #g#${3}#n##o#Insgesamt: #g#${4}",
				["deathinfo_killed"] = "{1} hat {2} mit {3} getötet",
				["deathinfo_died"] = "{1} ist gestorben",
				["round_mission_normal"] = "Ziel: Alle Gegner müssen getötet werden.",
				["round_mission_bomb_spectator"] = "Ziel: Bombe legen & verteidigen oder entschärfen - oder alle Gegner töten.",
				["round_mission_bomb_good"] = "Ziel: Verhinde die Bomben-Explosion oder töte alle Gegner!",
				["round_mission_bomb_bad"] = "Ziel: Lass die Bombe an einem der Punkte explodieren oder töte alle Gegner!",
				["bomb_planted"] = "Bombe wurde platziert!",
				["killing_spree_healtharmor"] = "{1} hat einen {2}er Killingspree und kriegt dafür {3} Leben/Weste.",
				["plant_info"] = "Um die Bombe zu platzieren, musst du zur Faust wechseln und die linke Maustaste auf einem der Bomben-Spots gedrückt halten.",
				["defuse_info_1"] = "Runden-Zeit hat sich verändert. Nun musst du entweder alle Gegner töten oder die Bombe entschärfen.",
				["defuse_info_2"] = "Um die Bombe zu entschärfen, gehe zum roten Punkt auf der Map (Bombe), wechsel zur Faust und halte die linke Maustaste gedrückt.",
				["welcome_1"] = "Willkommen auf dem #b#Team Deathmatch Server#w#.",
				["welcome_2"] = "Für Ankündigungen, Support, Bug-Meldung usw.",
				["welcome_3"] = "bitte unseren Discord-Server nutzen:",
				["welcome_4"] = "discord.gg/ntVnGFt",
				["welcome_5"] = "Du kannst den Cursor mit ENDE umschalten.",
				["welcome_6"] = "Wenn du dich nicht bewegen kannst, nutze ALT+TAB.",
				["welcome_7"] = "Viel Spaß wünscht das #b#TDS-Team#w#!",
				["order_attack"] = "{1}Angriff! Los los los!",
				["order_stay_back"] = "{1}Bleibt zurück!",
				["order_spread_out"] = "{1}Teilt euch auf!",
				["order_go_to_bomb"] = "{1}Geht zur Bombe!",
				["target_not_logged_in"] = "Das Ziel ist nicht eingeloggt.",
				["target_not_in_same_lobby"] = "Das Ziel ist nicht in der selben Lobby.",
				["round_end_death"] = "Alle Gegner sind tot!<br>Team {1} gewinnt!",
				["round_end_death_all"] = "Alle sind tot!<br>Kein Team gewinnt!",
				["round_end_time"] = "Zeit um!<br>Team {1} gewinnt mit den meisten HP übrig!",
				["round_end_bomb_exploded"] = "Bombe explodiert!<br>Team {1} gewinnt!",
				["round_end_bomb_defused"] = "Bombe entschärft!<br>Team {1} gewinnt!",
				["round_end_command"] = "Die Map wurde von {1} übersprungen!",
				["round_end_new_player"] = "Genug Spieler drin ...<br>Runde startet!",
				["not_allowed"] = "Du bist dazu nicht befugt!",
				["gang_doesnt_exist_anymore"] = "Die Gang existiert nicht mehr!",
				["invitation_was_removed"] = "Die Einladung wurde bereits zurückgezogen!",
				["created_report"] = "Ein Report mit der ID {1} wurde erstellt!",
				["answered_report"] = "Der Ersteller hat seinem Report mit der ID {1} geantwortet.",
				["got_report_answer"] = "Ein Team-Mitglied hat deinem Report mit der ID {1} geantwortet.",
				["reason_missing"] = "Die Begründung fehlt!",
				["player_not_muted"] = "Der Spieler ist nicht gemutet!",
				["player_already_muted"] = "Der Spieler ist bereits gemutet!",
				["you_are_still_permamuted"] = "Du bist noch permanent gemutet.",
				["you_are_still_muted"] = "Du bist noch für {1} Minuten gemutet.",
				["not_possible_in_this_lobby"] = "In dieser Lobby nicht möglich!",
				["you_got_offline_messages_without_new"] = "Du hast {1} Offline-Nachrichten im Userpanel.",
				["you_got_offline_messages_with_new"] = "Du hast {1} Offline-Nachrichten im Userpanel, {2} davon sind neu.",
				["gang_removed"] = "Deine Gang wurde aufgelöst."
			}
		};

		private static StringBuilder builder = new StringBuilder();

		public static Dictionary<Language, string> GetLangDictionary(string type, params string[] args)
		{
			Dictionary<Language, string> returndict = new Dictionary<Language, string>();
			foreach ( Language language in Enum.GetValues(typeof(Language)) )
			{
				returndict[language] = GetLang(language, type, args);
			}
			return returndict;
		}

		public static string GetLang(this Character character, string type, params string[] args)
		{
			Language language = character.Language;
			if ( args.Length == 0 )
				return langData[language][type];
			return GetReplaced(langData[language][type], args);
		}

		public static string GetLang(this Client player, string type, params string[] args)
		{
			return GetLang(player.GetChar(), type, args);
		}

		public static string GetLang(Language language, string type, params string[] args)
		{
			if ( args.Length == 0 )
				return langData[language][type];
			return GetReplaced(langData[language][type], args);
		}

		public static void SendLangMessage(this Character character, string type, params string[] args)
		{
			NAPI.Chat.SendChatMessageToPlayer(character.Player, character.GetLang(type, args));
		}

		public static void SendLangMessage(this Client player, string type, params string[] args)
		{
			NAPI.Chat.SendChatMessageToPlayer(player, player.GetLang(type, args));
		}

		public static void SendLangNotification(this Character character, string type, params string[] args)
		{
			NAPI.Notification.SendNotificationToPlayer(character.Player, character.GetLang(type, args));
		}

		public static void SendLangNotification(this Client player, string type, params string[] args)
		{
			NAPI.Notification.SendNotificationToPlayer(player, player.GetLang(type, args));
		}

		public static string GetReplaced(string str, params string[] args)
		{
			if ( args.Length > 0 )
			{
				builder.Append(str);
				for ( int i = 0; i < args.Length; ++i )
				{
					builder.Replace("{" + (i + 1) + "}", args[i]);
				}
				string result = builder.ToString();
				builder.Clear();
				return result;
			}
			return str;
		}

		public static void SendMessageToAll(string type, params string[] args)
		{
			Dictionary<Language, string> texts = GetLangDictionary(type, args);
			List<Client> players = NAPI.Pools.GetAllPlayers();
			foreach ( Client player in players )
			{
				NAPI.Chat.SendChatMessageToPlayer(player, texts[player.GetChar().Language]);
			}
		}

		public static void SendNotificationToAll(string type, params string[] args)
		{
			Dictionary<Language, string> texts = GetLangDictionary(type, args);
			List<Client> players = NAPI.Pools.GetAllPlayers();
			foreach ( Client player in players )
			{
				NAPI.Notification.SendNotificationToPlayer(player, texts[player.GetChar().Language]);
			}
		}
	}

}