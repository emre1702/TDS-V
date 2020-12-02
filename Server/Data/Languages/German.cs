using System;
using System.Collections.Generic;
using TDS.Server.Data.Interfaces;

namespace TDS.Server.Data.Languages
{
    public class German : English, ILanguage
    {
        public override string A_MAP_WAS_ALREADY_BOUGHT => "Eine Karte wurde bereits gekauft/gesetzt.";
        public override string ACCOUNT_DOESNT_EXIST => "Account existiert nicht.";
        public override string ADDED_THE_GANG_HOUSE_SUCCESSFULLY => "Das Gang-Haus wurde erfolgreich hinzugefügt.";
        public override string ALREADY_IN_PRIVATE_CHAT_WITH => "Du bist bereits in einem Privatchat mit {0}.";

        public override string BALANCE_TEAM_INFO => "{0} wurde in ein anderes Team gesteckt, um die Teams auszugleichen.";
        public override string BOMB_PLANT_INFO => "Um die Bombe zu platzieren, musst du zur Faust wechseln und die linke Maustaste auf einem der Bomben-Spots gedrückt halten.";
        public override string BOMB_PLANTED => "Bombe wurde platziert.";

        public override string CHAR_IN_NAME_IS_NOT_ALLOWED => "Das Zeichen '{0}' in deinem Namen ist nicht erlaubt.";
        public override string COMMAND_DOESNT_EXIST => "Der Befehl existiert nicht.";
        public override string COMMAND_TOO_LESS_ARGUMENTS => "Du hast zu wenige Argumente für diesen Befehl angegeben. Erwartet werden {0}, aber gegeben sind {1} Argumente.";
        public override string COMMAND_TOO_MANY_ARGUMENTS => "Du hast zu viele Argumente für diesen Befehl angegeben. Erwartet werden {0}, aber gegeben sind {1} Argumente.";
        public override string COMMAND_USED_WRONG => "Der Befehl wurde falsch benutzt.";
        public override string COMMITED_SUICIDE => "Du hast Selbstmord begangen.";
        public override string CONNECTING => "Verbindet ...";
        public override string CUSTOM_LOBBY_CREATOR_NAME_ALREADY_TAKEN_ERROR => "Dieser Name ist bereits in Benutzung.";
        public override string CUSTOM_LOBBY_CREATOR_NAME_NOT_ALLOWED_ERROR => "Dieser Name ist nicht erlaubt für Lobbies.";
        public override string CUSTOM_LOBBY_CREATOR_UNKNOWN_ERROR => "Unbekannter Fehler beim Erstellen der Lobby.";

        public override string DEATH_DIED_INFO => "{0} ist gestorben";
        public override string DEATH_KILLED_INFO => "{0} hat {1} mit {2} getötet";

        public override List<string> DEFUSE_INFO => new List<string>
        {
            "Runden-Zeit hat sich verändert. Nun musst du entweder alle Gegner töten oder die Bombe entschärfen.",
            "Um die Bombe zu entschärfen, gehe zum roten Punkt auf der Map (Bombe), wechsel zur Faust und halte die linke Maustaste gedrückt."
        };

        public override string DISCORD_IDENTITY_CHANGED_BONUSBOT_INFO => "Der Spieler '{0}' hat deine Discord Benutzer-Id in TDS-V eingestellt."
            + Environment.NewLine + "Warst du dieser Spieler?"
            + Environment.NewLine + "Bitte bestätige die Einstellung mit dem Befehl '!confirmtds'.";

        public override string DISCORD_IDENTITY_SAVE_FAILED => "Speichern der Discord Benutzer-Id ist fehlgeschlagen: {0}";
        public override string DISCORD_IDENTITY_SAVED_SUCCESSFULLY => "Die Discord Benutzer-Id wurde erfolgreich gespeichert.";

        public override string ERROR_INFO => "Ein Fehler ist aufgetaucht. Der/die Entwickler wurde(n) benachrichtigt.";

        public override string FRIEND_JOINED_LOBBY_INFO => "Dein Freund {0} hat die Lobby '{1}' betreten.";
        public override string FRIEND_LEFT_LOBBY_INFO => "Dein Freund {0} hat die Lobby '{1}' verlassen.";
        public override string FRIEND_LOGGEDIN_INFO => "~g~Dein Freund {0} hat sich eingeloggt.";
        public override string FRIEND_LOGGEDOFF_INFO => "~r~Dein Freund {0} hat sich ausgeloggt.";
        public override string GANG_DOESNT_EXIST_ANYMORE => "Die Gang existiert nicht mehr!";
        public override string GANG_INVITATION_INFO => "Du hast eine Gang-Einladung.";
        public override string GANG_INVITATION_WAS_REMOVED => "Die Einladung wurde bereits zurückgezogen!";
        public override string GANG_LEVEL_MAX_ALLOWED => "Als Gang-Level ist höchstens {0} erlaubt.";
        public override string GANG_REMOVED => "Deine Gang wurde aufgelöst.";
        public override string GANGACTION_CANT_JOIN_AGAIN => "Du darfst der Gang-Aktion nicht erneut beitreten.";
        public override string GANGWAR_STARTED_INFO => "Die Gang '{0}' greift das Gebiet '{1}' von '{2}' an.";
        public override string GIVE_MONEY_NEED_FEE => "Du brauchst ${0} mit ${1} Gebühr inklusive.";
        public override string GIVE_MONEY_TOO_LESS => "Das Geld, was du versuchst zu geben, ist zu wenig.";
        public override string GOT_ASSIST => "Du hast den Assist von {0} bekommen.";
        public override string GOT_LAST_HITTED_KILL => "Du hast {0} zuletzt getroffen und den Kill bekommen.";
        public override string GOT_LOBBY_BAN => "Du hast hier vom Lobby-Besitzer einen Bann. Dauer: {0} | Grund: {1}";
        public override string GOT_UNREAD_OFFLINE_MESSAGES => "Du hast {0} ungelesene Offline-Nachricht(en) im Userpanel.";
        public override string HITSOUND_ACTIVATED => "Hitsound aktiviert!";
        public override string HITSOUND_DEACTIVATED => "Hitsound deaktiviert!";

        public override string INVITATION_LOBBY => "Du wurdest von {0} in die '{1}' Lobby eingeladen.";
        public override string INVITATION_WAS_WITHDRAWN_OR_REMOVED => "Die Einladung wurde schon zurückgezogen oder entfernt.";

        public override string JOINED_LOBBY_MESSAGE => "Du bist in die Lobby \"{0}\" eingetreten.\nNutze '/{1}' zum Verlassen.";

        public override string KICK_INFO => "{0} wurde von {1} gekickt. Grund: {2}";
        public override string KICK_LOBBY_INFO => "{0} wurde von {1} aus der Lobby gekickt. Grund: {2}";
        public override string KICK_YOU_INFO => "Du wurdest von {0} gekickt. Grund: {1}";
        public override string KILLING_SPREE_HEALTHARMOR => "{0} hat einen {1}er Killingspree und kriegt dafür {2} Leben/Weste.";

        public override string LOBBY_DOESNT_EXIST => "Diese Lobby existiert nicht.";
        public override string LOBBY_ERROR_REMOVE => "Die Lobby hatte einen Fehler und wurde entfernt. Der/die Entwickler wurde(n) benachrichtigt.";

        public override string MAP_BUY_INFO => "{0} hat die Karte {1} gekauft.";
        public override string MAP_WON_VOTING => "Die Map {0} hat das Voting gewonnen!";
        public override string MUTE_EXPIRED => "Dein Chat Mute ist abgelaufen. Du darfst wieder schreiben.";
        public override string MUTETIME_INVALID => "Die Mute-Zeit ist ungültig. Erlaubt sind -1, 0 und höher.";

        public override string NEW_OFFLINE_MESSAGE => "Du hast eine neue Offline-Nachricht bekommen.";
        public override string NOT_ALLOWED => "Du bist dazu nicht befugt.";
        public override string NOT_ENOUGH_MONEY => "Du hast nicht genug Geld.";
        public override string NOT_IN_PRIVATE_CHAT => "Du bist nicht in einem Privatchat.";
        public override string NOT_MORE_MAPS_FOR_VOTING_ALLOWED => "Es dürfen nur 6 Maps im Voting sein!";
        public override string NOT_POSSIBLE_IN_THIS_LOBBY => "In dieser Lobby nicht möglich!";

        public override string ONLY_ALLOWED_IN_GANG_LOBBY => "Das ist nur in der Gang-Lobby erlaubt.";
        public override string ORDER_ATTACK => "Angriff! Los los los!";
        public override string ORDER_GO_TO_BOMB => "Geht zur Bombe!";
        public override string ORDER_SPREAD_OUT => "Teilt euch auf!";
        public override string ORDER_STAY_BACK => "Bleibt zurück!";

        public override string PERMABAN_INFO => "{0} wurde permanent von {1} gebannt. Grund: {2}";
        public override string PERMABAN_LOBBY_INFO => "{0} wurde permanent aus der Lobby '{1}' von {2} gebannt. Grund: {3}";
        public override string PERMABAN_LOBBY_YOU_INFO => "Du wurdest permanent aus der Lobby '{0}' von {1} gebannt. Grund {2}";
        public override string PERMABAN_YOU_INFO => "Du wurdest permanent von {0} gebannt. Grund: {1}";
        public override string PERMAMUTE_INFO => "{0} wurde von {1} permanent gemutet. Grund: {2}";
        public override string PERMAVOICEMUTE_INFO => "{0} wurde von {1} im Voice-Chat permanent gemutet. Grund: {2}";
        public override string PLAYER_ACCEPTED_YOUR_INVITATION => "{0} hat deine Team-Einladung angenommen. Er ist nun dein Mitglied.";
        public override string PLAYER_ALREADY_MUTED => "Der Spieler ist bereits gemutet!";
        public override string PLAYER_ALREADY_ON_THIS_PAGE => "{0} ist bereits auf dieser Seite. Nur eine Person darf diese Seite gleichzeitig bearbeiten.";
        public override string PLAYER_DOESNT_EXIST => "Der Spieler existiert nicht!";
        public override string PLAYER_ISNT_BANED => "Der Spieler ist nicht gebannt.";
        public override string PLAYER_JOINED_YOUR_GANG => "{0} ist eurer Gang beigetreten.";
        public override string PLAYER_LOGGED_IN => "~b~~h~{0}~h~ ~w~hat sich eingeloggt.";
        public override string PLAYER_LOGGED_OUT => "~b~~h~{0}~h~ ~w~hat sich ausgeloggt.";
        public override string PLAYER_NOT_IN_YOUR_GANG => "Das Ziel ist nicht (mehr) in eurer Gang.";
        public override string PLAYER_NOT_MUTED => "Der Spieler ist nicht gemutet!";
        public override string PLAYER_REGISTERED => "Ein neuer Spieler mit dem Namen \"{0}\" hat sich registriert!";
        public override string PLAYER_REJECTED_YOUR_INVITATION => "{0} hat deine Team-Einladung abgelehnt.";
        public override string PLAYER_WITH_NAME_ALREADY_EXISTS => "Es existiert bereits ein Spieler mit diesem Namen.";
        public override string PLAYER_WON_INFO => "{0} hat die Runde gewonnen.";
        public override string PRIVATE_CHAT_CLOSED_PARTNER => "Dein Privatchat-Partner hat den Chat geschlossen.";
        public override string PRIVATE_CHAT_CLOSED_YOU => "Du hast den Privatchat geschlossen.";
        public override string PRIVATE_CHAT_DISCONNECTED => "Dein Privatchat-Partner ist disconnectet.";
        public override string PRIVATE_CHAT_OPENED_WITH => "Privatchat mit {0} wurde geöffnet.";
        public override string PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER => "Die Chat-Anfrage von {0} wurde zurückgezogen.";
        public override string PRIVATE_CHAT_REQUEST_CLOSED_YOU => "Du hast die Chat-Anfrage zurückgezogen.";
        public override string PRIVATE_CHAT_REQUEST_RECEIVED_FROM => "Du hast eine Privatchat-Anfrage von {0} bekommen.";
        public override string PRIVATE_CHAT_REQUEST_SENT_TO => "Du hast eine Privatchat-Anfrage an {0} gesendet.";
        public override string PRIVATE_MESSAGE_SENT => "Die Privat-Nachricht wurde gesendet.";

        public override string REASON_MISSING => "Die Begründung fehlt!";
        public override string REPORT_ANSWERED_INFO => "Der Ersteller hat seinem Report mit der ID {0} geantwortet.";
        public override string REPORT_CREATED_INFO => "Ein Report mit der ID {0} wurde erstellt!";
        public override string REPORT_GOT_ANSWER_INFO => "Ein Team-Mitglied hat deinem Report mit der ID {0} geantwortet.";
        public override string RESOURCE_RESTART_INFO_MINUTES => "TDS-V wird in {0} Minute(n) neu gestartet.";
        public override string RESOURCE_RESTART_INFO_SECONDS => "TDS-V wird in {0} Sekunde(n) neu gestartet.";
        public override string ROUND_END_BOMB_DEFUSED_INFO => "Bombe entschärft!<br>Team {0} gewinnt!";
        public override string ROUND_END_BOMB_EXPLODED_INFO => "Bombe explodiert!<br>Team {0} gewinnt!";
        public override string ROUND_END_COMMAND_INFO => "Die Map wurde von {0} übersprungen!";
        public override string ROUND_END_DEATH_ALL_INFO => "Alle sind tot!<br>Kein Team gewinnt!";
        public override string ROUND_END_DEATH_INFO => "Alle Gegner sind tot!<br>Team {0} gewinnt!";
        public override string ROUND_END_NEW_PLAYER_INFO => "Genug Spieler drin ...<br>Runde startet!";
        public override string ROUND_END_TARGET_EMPTY_INFO => "Die Angreifer waren zu lange nicht mehr am Ziel.<br>Besitzer gewinnen!";
        public override string ROUND_END_TIME_INFO => "Zeit um!<br>Team {0} gewinnt mit den meisten HP übrig!";
        public override string ROUND_END_TIME_TIE_INFO => "Zeit um!<br>Unentschieden!";
        public override string ROUND_MISSION_BOMB_BAD => "Ziel: Lass die Bombe an einem der Punkte explodieren oder töte alle Gegner!";
        public override string ROUND_MISSION_BOMB_GOOD => "Ziel: Verhindere die Bomben-Explosion oder töte alle Gegner!";
        public override string ROUND_MISSION_BOMG_SPECTATOR => "Ziel: Bombe legen & verteidigen oder entschärfen - oder alle Gegner töten.";
        public override string ROUND_MISSION_NORMAL => "Ziel: Alle Gegner müssen getötet werden.";
        public override string ROUND_REWARD_INFO => "#w#Runden-Belohnung:#n#Kills: #g#${0}#n##w#Assists: #g#${1}#n##w#Damage: #g#${2}#n##o#Insgesamt: #g#${3}";

        public override string SENT_APPLICATION => "Du hast eine Einladung gesendet.";
        public override string SENT_APPLICATION_TO => "Du hast {0} eine Einladung gesendet.";
        public override string STILL_BANED_IN_LOBBY => "Du hast noch einen Ban bis {0} von {1}. Grund: {2}";
        public override string STILL_MUTED => "Du bist noch für {0} Minuten gemutet.";
        public override string STILL_PERMABANED_IN_LOBBY => "Du wurdest von {0} aus dieser Lobby permanent gebannt. Grund: {1}";
        public override string STILL_PERMAMUTED => "Du bist noch permanent gemutet.";
        public override string SUPPORT_REQUEST_CREATED => "Die Support-Anfrage wurde erstellt.";

        public override string TARGET_ACCEPTED_INVITATION => "{0} hat deine Einladung angenommen.";
        public override string TARGET_ADDED_BLOCK => "Der Spieler {0} wurde von dir geblockt.";
        public override string TARGET_ALREADY_BLOCKED => "Der Spieler {0} wurde bereits von dir geblockt.";
        public override string TARGET_ALREADY_IN_A_GANG => "Das Ziel ist bereits in einer Gang.";
        public override string TARGET_ALREADY_IN_PRIVATE_CHAT => "Das Ziel ist bereits in einem Privatchat.";
        public override string TARGET_NOT_BLOCKED => "{0} ist nicht blockiert.";
        public override string TARGET_NOT_IN_SAME_LOBBY => "Das Ziel ist nicht in der selben Lobby.";
        public override string TARGET_NOT_LOGGED_IN => "Das Ziel ist nicht eingeloggt.";
        public override string TARGET_PLAYER_DEFEND_INFO => "Euer Teamkollege {0} ist am Ziel. Verteidigt ihn!";
        public override string TARGET_RANK_IS_HIGHER_OR_EQUAL => "Der Rang des Ziels ist höher als oder gleichwertig wie dein Rang.";
        public override string TARGET_REJECTED_INVITATION => "{0} hat deine Einladung abgelehnt.";
        public override string TARGET_REMOVED_FRIEND_ADDED_BLOCK => "Der Spieler {0} ist nicht mehr dein Freund und wurde nun geblockt.";
        public override string TEAM_ALREADY_FULL_INFO => "Dein Team ist bereits voll. Warte darauf, dass ein Gegner beitritt, und versuche es dann erneut.";
        public override string TESTING_MAP_NOTIFICATION => "Das ist eine neu erstellste Karte, die Runden-Statistiken werden nicht gespeichert werden.";
        public override string TEXT_TOO_LONG => "Der Text ist zu lang.";
        public override string TEXT_TOO_SHORT => "Der Text ist zu kurz.";
        public override string THE_RANK_IS_INVALID_REFRESH_WINDOW => "Der Rang ist ungültig. Bitte aktualisiere das Fenster.";
        public override string TIMEBAN_INFO => "{0} wurde für {1} Stunden von {2} gebannt. Grund: {3}";
        public override string TIMEBAN_LOBBY_INFO => "{0} wurde für {1} Stunden aus der Lobby '{2}' von {3} gebannt. Grund: {4}";
        public override string TIMEBAN_LOBBY_YOU_INFO => "Du wurdest für {0} Stunden aus der Lobby '{1}' von {2} gebannt. Grund {3}";
        public override string TIMEBAN_YOU_INFO => "Du wurdest für {0} Stunden von {1} gebannt. Grund: {2}";
        public override string TIMEMUTE_INFO => "{0} wurde von {1} für {2} Minuten gemutet. Grund: {3}";
        public override string TIMEVOICEMUTE_INFO => "{0} wurde von {1} für {2} Minuten im Voice-Chat gemutet. Grund: {3}";
        public override string TOO_LONG_OUTSIDE_MAP => "Du warst zu lange außerhalb der Map.";
        public override string TRY_AGAIN_LATER => "Versuche es später erneut.";

        public override string UNBAN_INFO => "{0} wurde von {1} entbannt. Grund: {2}";
        public override string UNBAN_LOBBY_INFO => "{0} wurde in der Lobby '{1}' von {2} entbannt. Grund: {3}";
        public override string UNBAN_YOU_LOBBY_INFO => "Du wurdest in der Lobby '{0}' von {1} entbannt. Grund: {2}";
        public override string UNMUTE_INFO => "{0} wurde von {1} entmutet. Grund: {2}";
        public override string UNVOICEMUTE_INFO => "{0} wurde von {1} im Voice-Chat entmutet. Grund: {2}";

        public override string VOICE_MUTE_EXPIRED => "Dein Voice-Channel Mute ist abgelaufen. Du darfst wieder reden.";

        public override string WRONG_PASSWORD => "Falsches Passwort!";

        public override string YOU_ACCEPTED_INVITATION => "Du hast die Einladung von {0} angenommen.";
        public override string YOU_ACCEPTED_TEAM_INVITATION => "Du hast die Team-Einladung angenommen. {0} ist nun dein Team-Leiter.";
        public override string YOU_ALREADY_INVITED_TARGET => "Du hast das Ziel bereits eingeladen.";
        public override string YOU_ARE_ALREADY_IN_A_GANG => "Du bist bereits in einer Gang.";
        public override string YOU_ARE_NOT_IN_A_GANG => "Du bist nicht in einer Gang.";
        public override string YOU_CANT_BUY_A_MAP_IN_CUSTOM_LOBBY => "Du kannst keine Karte in einer Benutzer-Lobby kaufen.";
        public override string YOU_GAVE_MONEY_TO_WITH_FEE => "Du hast {2} ${0} (${1} Gebühr) gegeben.";
        public override string YOU_GOT_BLOCKED_BY => "Du wurdest von {0} geblockt.";
        public override string YOU_GOT_INVITATION_BY => "Du hast von {0} eine Einladung ins Team bekommen.";
        public override string YOU_GOT_KICKED_OUT_OF_THE_GANG_BY => "Du wurdest von {0} aus der Gang '{1}' geworfen.";
        public override string YOU_GOT_MONEY_BY_WITH_FEE => "Du hast ${0} (${1} Gebühr) von {2} bekommen.";
        public override string YOU_GOT_RANK_DOWN_BY => "Du hast von {0} ein Rank-Down von {1} auf {2} bekommen.";
        public override string YOU_GOT_RANK_UP => "Du hast von {0} ein Rank-Up von {1} auf {2} bekommen.";
        public override string YOU_GOT_UNBLOCKED_BY => "Du wurdest von {0} entblockt.";
        public override string YOU_JOINED_THE_GANG => "Du bist der Gang '{0}' beigetreten.";
        public override string YOU_KICKED_OUT_OF_GANG => "Du hast {0} aus der Gang geworfen.";
        public override string YOU_REJECTED_GANG_INVITATION => "Du hast die Einladung von der Gang '{0}' abgelehnt.";
        public override string YOU_REJECTED_INVITATION => "Du hast die Einladung von {0} abgelehnt.";
        public override string YOU_REJECTED_TEAM_INVITATION => "Du hast die Team-Einladung von {0} abgelehnt.";
        public override string YOU_UNBLOCKED => "Du hast {0} nicht mehr blockiert.";
        public override string YOUVE_BECOME_GANG_LEADER => "Du wurdest zum Gang-Besitzer.";
        public override string YOUVE_LEFT_THE_GANG => "Du hast die Gang verlassen.";
        public override string NO_EMAIL_ADDRESS_HAS_BEEN_SET => "Dieser Account hat keine E-Mail Adresse hinterlegt.";
        public override string EMAIL_ADDRESS_FOR_ACCOUNT_IS_INVALID => "Die für diesen Account hinterlegte E-Mail Adresse ist ungültig.";
        public override string PASSWORD_HAS_BEEN_RESET_EMAIL_SENT => "Dein Passwort wurde erfolgreich zurückgesetzt. Überprüfe deine E-Mails (besonders die Junk-Emails!).";
        public override string REGISTER_FAILED_DEVS_INFORMED => "Ein Fehler ist beim Registrieren aufgetreten. Die Entwickler sind informiert und werden das Problem so schnell es geht beheben. Versuche es bitte später erneut.";
        public override string GANGWAR_TARGET_EMPTY_SECS_LEFT => "Das Ziel ist leer! Ihr habt {0} Sekunden, um es wieder zu besetzen.";
        public override string GANG_ACTION_IN_PREPARATION => "Ein Angriff wird im Gebiet {0} vorbereitet.";
        public override string GANG_ACTION_IN_PREPARATION_ATTACKER => "Deine Gang bereitet einen Angriff gegen {0} im Gebiet {1} vor.";
        public override string GANG_ACTION_IN_PREPARATION_OWNER => "Eine Gang bereitet einen Angriff in eurem Gebiet {0} vor.";
        public override string GANG_ACTION_STARTED => "Ein Angriff wurde von {0} gegen {1} gestartet. Gebiet: {2}";
        public override string GANG_AREA_CONQUERED_WITH_OWNER => "Das Gebiet {0} wurde erfolgreich von {1} vom Besitzer {2} erobert.";
        public override string GANG_AREA_CONQUERED_WITHOUT_OWNER => "Das Gebiet {0} wurde erfolgreich von {1} genommen.";
        public override string GANG_AREA_DEFENDED => "Das Gebiet {0} wurde erfolgreich von {1} gegen die Angreifer {2} verteidigt.";
        public override string GANG_ACTION_AREA_IN_COOLDOWN => "Das Gebiet hat einen Cooldown. Versuch es nach {0} Minuten erneut.";
        public override string GANG_ACTION_AREA_OWNER_IN_ACTION => "Der Besitzer des Gebietes ist bereits in einer Aktion.";
        public override string GANG_ACTION_ATTACK_PREPARATION_INVITATION => "Eure Gang bereitet einen Angriff gegen das Gebiet {0} vor!\n(Nimm an oder nutze /angreifen)";
        public override string GANG_ACTION_ATTACK_INVITATION => "Your gang is attacking area {0}!\n(Nimm an oder nutze /angreifen)";
        public override string GANG_ACTION_DEFEND_INVITATION => "Defend your gang area {0}!\n(Nimm an oder nutze /verteidigen)";
        public override string GANG_ACTION_TEAM_YOURS_PLAYER_JOINED_INFO => "{0} ist der Gang-Aktion für eure Gang beigetreten.";
        public override string GANG_ACTION_TEAM_OPPONENT_PLAYER_JOINED_INFO => "{0} ist der Gang-Aktion für die Gegner beigetreten.";
        public override string YOUR_GANG_ALREADY_REACHED_MAX_ATTACK_COUNT => "Eure Gang hat bereits die max. Anzahl an Attacks von {0} für heute erreicht.";
        public override string YOUR_GANG_ALREADY_IN_ACTION => "Eure Gang ist bereits in einer Gang-Aktion.";
        public override string NOT_ENOUGH_PLAYERS_ONLINE_IN_YOUR_GANG => "Es sind nicht genug Spieler online in eurer Gang (erwartet: {0}).";
        public override string GANG_ACTION_AREA_DOES_NOT_EXIST => "Das Gang-Gebiet existiert nicht?!";
        public override string NOT_ENOUGH_PLAYERS_ONLINE_IN_TARGET_GANG => "Es sind nicht genug Spieler online in der gegnerischen Gang (erwartet: {0}).";
        public override string ADMIN_LEVEL_MIN_NUMBER => "Das Admin-Level darf mindestens {0} sein.";
        public override string ADMIN_LEVEL_MAX_NUMBER => "Das Admin-Level darf höchstens {0} sein.";
        public override string MIN_NUMBER => "Die Zahl darf mindestens {0} sein.";
        public override string MAX_NUMBER => "Die Zahl darf höchstens {0} sein.";
        public override string ADMIN_LEVEL_SET_SUCCESSFULLY => "Das Admin-Level von {0} wurde erfolgreich auf {1} gesetzt.";
        public override string ADMIN_HAS_SET_YOUR_ADMIN_LEVEL => "Der Admin {0} hat dein Admin-Level auf {1} gesetzt.";
        public override string SHOULD_SET_ADMIN_LEADER_INFO => "Der Spieler ist hierarchisch nicht direkt unter dir, aber du bist sein Vorgesetzter. Du solltest seinen Admin-Leader auf einen direkten Vorgesetzten setzen.";
        public override string ADMIN_LEADER_SET_SUCCESSFULLY => "Du hast {1} zum Vorgesetzten von {0} gemacht.";
        public override string ADMIN_HAS_SET_YOUR_ADMIN_LEADER => "{0} hat {1} zu deinem Vorgesetzten gemacht.";
        public override string YOUVE_BECOME_ADMIN_LEADER => "{0} hat dich zum Vorgesetzten von {1} gemacht.";
        public override string VIP_SET_SUCCESSFULLY => "Du hast isVip von {0} auf {1} gesetzt.";
        public override string ADMIN_HAS_SET_YOU_VIP => "{0} hat dir den VIP-Rang gegeben. Glückwunsch!";
        public override string ADMIN_HAS_SET_YOU_NOT_VIP => "{0} hat dir den VIP-Rang weggenommen.";
        public override string SUPERVISOR_OF_PLAYER => "Der Vorgesetzte vom Spieler ist {0}.";
        public override string TARGET_IS_ADMIN_BUT_HAS_NO_SUPERVISOR => "Der Spieler ist Admin, aber hat keinen Vorgesetzten.";
        public override string TARGET_IS_NOT_ADMIN => "Der Spieler ist kein Admin.";
        public override string MAP_CREATOR_REWARD_INFO => "Der Ersteller der Karte '{0}' hat ${1} bekommen.";
        public override string YOU_GOT_MAP_CREATOR_REWARD => "Deine Karte '{0}' wurde ausgewählt -> du verdienst ${1}.";
    }
}
