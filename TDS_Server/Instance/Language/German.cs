using TDS_Server.Interface;

namespace TDS_Server.Instance.Language
{
    internal class German : English, ILanguage
    {
        public override string ACCOUNT_DOESNT_EXIST => "Account existiert nicht.";
        public override string ADMINLVL_NOT_HIGH_ENOUGH => "Dein Adminlevel ist nicht hoch genug dafür.";
        public override string ALREADY_IN_PRIVATE_CHAT_WITH => "Du bist bereits in einem Privatchat mit {0}.";

        public override string BOMB_PLANT_INFO => "Um die Bombe zu platzieren, musst du zur Faust wechseln und die linke Maustaste auf einem der Bomben-Spots gedrückt halten.";
        public override string BOMB_PLANTED => "Bombe wurde platziert.";

        public override string COMMAND_DOESNT_EXIST => "Der Befehl existiert nicht.";
        public override string COMMAND_TOO_LESS_ARGUMENTS => "Du hast zu wenige Argumente für diesen Befehl angegeben.";
        public override string COMMAND_USED_WRONG => "Der Befehl wurde falsch benutzt.";
        public override string COMMITED_SUICIDE => "Du hast Selbstmord begangen.";
        public override string CONNECTING => "Verbindet ...";

        public override string DEATH_KILLED_INFO => "{0} hat {1} mit {2} getötet";
        public override string DEATH_DIED_INFO => "{0} ist gestorben";

        public override string[] DEFUSE_INFO => new string[]
        {
            "Runden-Zeit hat sich verändert. Nun musst du entweder alle Gegner töten oder die Bombe entschärfen.",
            "Um die Bombe zu entschärfen, gehe zum roten Punkt auf der Map (Bombe), wechsel zur Faust und halte die linke Maustaste gedrückt."
        };

        public override string GANG_DOESNT_EXIST_ANYMORE => "Die Gang existiert nicht mehr!";
        public override string GANG_REMOVED => "Deine Gang wurde aufgelöst.";
        public override string GOT_ASSIST => "Du hast den Assist von {0} bekommen.";
        public override string GOT_LAST_HITTED_KILL => "Du hast {0} zuletzt getroffen und den Kill bekommen.";
        public override string GOT_LOBBY_BAN => "Du hast hier vom Lobby-Besitzer einen Bann. Dauer: {0} | Grund: {1}";
        public override string GOT_UNREAD_OFFLINE_MESSAGES => "Du hast {0} ungelesene Offline-Nachricht(en) im Userpanel.";
        public override string GANG_INVITATION_WAS_REMOVED => "Die Einladung wurde bereits zurückgezogen!";

        public override string HITSOUND_ACTIVATED => "Hitsound aktiviert!";
        public override string HITSOUND_DEACTIVATED => "Hitsound deaktiviert!";

        public override string KICK_INFO => "{0} wurde von {1} gekickt. Grund: {2}";
        public override string KICK_YOU_INFO => "Du wurdest von {0} gekickt. Grund: {1}";
        public override string KICK_LOBBY_INFO => "{0} wurde von {1} aus der Lobby gekickt. Grund: {2}";
        public override string KICK_LOBBY_YOU_INFO => "Du wurdest von {0} aus der Lobby gekickt. Grund: {1}";
        public override string KILLING_SPREE_HEALTHARMOR => "{0} hat einen {1}er Killingspree und kriegt dafür {2} Leben/Weste.";

        public override string LOBBY_DOESNT_EXIST => "Diese Lobby existiert nicht.";
        public override string LOBBY_ERROR_REMOVE => "Die Lobby hatte einen Fehler und wurde entfernt. Der/die Entwickler wurde(n) benachrichtigt.";

        public override string MAP_WON_VOTING => "Die Map {0} hat das Voting gewonnen!";
        public override string MUTETIME_INVALID => "Die Mute-Zeit ist ungültig. Erlaubt sind -1, 0 und höher.";

        public override string NOT_ALLOWED => "Du bist dazu nicht befugt!";
        public override string NOT_IN_PRIVATE_CHAT => "Du bist nicht in einem Privatchat.";
        public override string NOT_MORE_MAPS_FOR_VOTING_ALLOWED => "Es dürfen nur 6 Maps im Voting sein!";
        public override string NOT_POSSIBLE_IN_THIS_LOBBY => "In dieser Lobby nicht möglich!";

        public override string ORDER_ATTACK => "Angriff! Los los los!";
        public override string ORDER_GO_TO_BOMB => "Geht zur Bombe!";
        public override string ORDER_SPREAD_OUT => "Teilt euch auf!";
        public override string ORDER_STAY_BACK => "Bleibt zurück!";

        public override string PERMABAN_INFO => "{0} wurde permanent von {1} gebannt. Grund: {2}";
        public override string PERMABAN_YOU_INFO => "Du wurdest permanent von {0} gebannt. Grund: {1}";
        public override string PERMABAN_LOBBY_INFO => "{0} wurde permanent aus der Lobby '{1}' von {2} gebannt. Grund: {3}";
        public override string PERMABAN_LOBBY_YOU_INFO => "Du wurdest permanent aus der Lobby '{0}' von {1} gebannt. Grund {2}";
        public override string PERMAMUTE_INFO => "{0} wurde von {1} permanent gemutet. Grund: {2}";
        public override string PLAYER_ALREADY_MUTED => "Der Spieler ist bereits gemutet!";
        public override string PLAYER_DOESNT_EXIST => "Der Spieler existiert nicht!";
        public override string PLAYER_ISNT_BANED => "Der Spieler ist nicht gebannt.";
        public override string PLAYER_NOT_MUTED => "Der Spieler ist nicht gemutet!";
        public override string PRIVATE_CHAT_CLOSED_PARTNER => "Dein Privatchat-Partner hat den Chat geschlossen.";
        public override string PRIVATE_CHAT_CLOSED_YOU => "Du hast den Privatchat geschlossen.";
        public override string PRIVATE_CHAT_DISCONNECTED => "Dein Privatchat-Partner ist disconnectet.";
        public override string PRIVATE_CHAT_OPENED_WITH => "Privatchat mit {0} wurde geöffnet.";
        public override string PRIVATE_CHAT_REQUEST_CLOSED_REQUESTER => "Die Chat-Anfrage von {0} wurde zurückgezogen.";
        public override string PRIVATE_CHAT_REQUEST_CLOSED_YOU => "Du hast die Chat-Anfrage zurückgezogen.";
        public override string PRIVATE_CHAT_REQUEST_RECEIVED_FROM => "Du hast eine Privatchat-Anfrage von {0} bekommen.";
        public override string PRIVATE_CHAT_REQUEST_SENT_TO => "Du hast eine Privatchat-Anfrage an {0} gesendet.";

        public override string REASON_MISSING => "Die Begründung fehlt!";
        public override string REPORT_ANSWERED_INFO => "Der Ersteller hat seinem Report mit der ID {0} geantwortet.";
        public override string REPORT_CREATED_INFO => "Ein Report mit der ID {0} wurde erstellt!";
        public override string REPORT_GOT_ANSWER_INFO => "Ein Team-Mitglied hat deinem Report mit der ID {0} geantwortet.";
        public override string ROUND_END_BOMB_DEFUSED_INFO => "Bombe entschärft!<br>Team {0} gewinnt!";
        public override string ROUND_END_BOMB_EXPLODED_INFO => "Bombe explodiert!<br>Team {0} gewinnt!";
        public override string ROUND_END_COMMAND_INFO => "Die Map wurde von {0} übersprungen!";
        public override string ROUND_END_DEATH_INFO => "Alle Gegner sind tot!<br>Team {0} gewinnt!";
        public override string ROUND_END_DEATH_ALL_INFO => "Alle sind tot!<br>Kein Team gewinnt!";
        public override string ROUND_END_NEW_PLAYER_INFO => "Genug Spieler drin ...<br>Runde startet!";
        public override string ROUND_END_TIME_INFO => "Zeit um!<br>Team {0} gewinnt mit den meisten HP übrig!";
        public override string ROUND_END_TIME_TIE_INFO => "Zeit um!<br>Unentschieden!";
        public override string ROUND_MISSION_BOMB_BAD => "Ziel: Lass die Bombe an einem der Punkte explodieren oder töte alle Gegner!";
        public override string ROUND_MISSION_BOMB_GOOD => "Ziel: Verhinde die Bomben-Explosion oder töte alle Gegner!";
        public override string ROUND_MISSION_BOMG_SPECTATOR => "Ziel: Bombe legen & verteidigen oder entschärfen - oder alle Gegner töten.";
        public override string ROUND_MISSION_NORMAL => "Ziel: Alle Gegner müssen getötet werden.";
        public override string ROUND_REWARD_INFO => "#w#Runden-Belohnung:#n#Kills: #g#${0}#n##w#Assists: #g#${1}#n##w#Damage: #g#${2}#n##o#Insgesamt: #g#${3}";

        public override string STILL_BANED_IN_LOBBY => "Du hast noch einen Ban bis {0} von {1}. Grund: {2}";
        public override string STILL_MUTED => "Du bist noch für {0} Minuten gemutet.";
        public override string STILL_PERMABANED_IN_LOBBY => "Du wurdest von {0} aus dieser Lobby permanent gebannt. Grund: {1}";
        public override string STILL_PERMAMUTED => "Du bist noch permanent gemutet.";

        public override string TARGET_ALREADY_IN_PRIVATE_CHAT => "Das Ziel ist bereits in einem Privatchat.";
        public override string TARGET_NOT_IN_SAME_LOBBY => "Das Ziel ist nicht in der selben Lobby.";
        public override string TARGET_NOT_LOGGED_IN => "Das Ziel ist nicht eingeloggt.";
        public override string TIMEBAN_INFO => "{0} wurde für {1} Stunden von {2} gebannt. Grund: {3}";
        public override string TIMEBAN_YOU_INFO => "Du wurdest für {0} Stunden von {1} gebannt. Grund: {2}";
        public override string TIMEBAN_LOBBY_INFO => "{0} wurde für {1} Stunden aus der Lobby '{2}' von {3} gebannt. Grund: {4}";
        public override string TIMEBAN_LOBBY_YOU_INFO => "Du wurdest für {0} Stunden aus der Lobby '{1}' von {2} gebannt. Grund {3}";
        public override string TIMEMUTE_INFO => "{0} wurde von {1} für {2} Minuten gemutet. Grund: {3}";
        public override string TOO_LONG_OUTSIDE_MAP => "Du warst zu lange außerhalb der Map.";

        public override string UNBAN_INFO => "{0} wurde von {1} entbannt. Grund: {2}";
        public override string UNBAN_LOBBY_INFO => "{0} wurde in der Lobby '{1}' von {2} entbannt. Grund: {3}";
        public override string UNBAN_YOU_LOBBY_INFO => "Du wurdest in der Lobby '{0}' von {1} entbannt. Grund: {2}";
        public override string UNMUTE_INFO => "{0} wurde von {1} entmutet. Grund: {2}";

        public override string[] WELCOME_MESSAGE => new string[] {
            "#n#Willkommen auf dem #b#Team Deathmatch Server#w#.",
            "#n#Für Ankündigungen, Support, Bug-Meldung usw.",
            "#n#bitte unseren Discord-Server nutzen:",
            "#n#discord.gg/ntVnGFt",
            "#n#Du kannst den Cursor mit ENDE umschalten.",
            "#n#Viel Spaß wünscht das #b#TDS-Team#w#!"
        };

        public override string WRONG_PASSWORD => "Falsches Passwort!";
    }
}