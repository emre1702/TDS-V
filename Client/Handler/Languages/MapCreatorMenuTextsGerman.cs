namespace TDS_Client.Handler.Entities.Languages
{
    internal class MapCreatorMenuTextsGerman : MapCreatorMenuTextsEnglish
    {
        #region Public Properties

        public override string ADD => "Add";
        public override string BOMB => "Bombe";
        public override string BOMB_PLACES => "Bomb-Spots";
        public override string BOMB_PLACES_MIN => "Es muss mindestens einen Bomben-Spot geben, um die Bombe platzieren zu können.";
        public override string CHECK => "Ueberpruefen";
        public override string DESCRIPTIONS => "Beschreibungen";
        public override string GENERAL => "Allgemein";
        public override string GO => "Gehe";
        public override string LAST => "Zuletzt";

        public override string LAST_INFO => @"Wenn du die Map sendest, wird sie auf dem Server gespeichert.<br>Dann bekommt Bonus die Chance die Map gründlich zu testen.<br>
            Wenn er Probleme oder Fehler findet, fixt er sie.<br>Wenn die Map ausreichend gut ist, wird sie spielbar gemacht.<br><br>
            Dieser ganze Prozess kann etwas dauern, daher solltest du mit min. 1-2 Tagen rechnen.<br>Bitte nutze zuerst 'Prüfen', dann verbessere wenn möglich die Map und danach kannst du 'Senden' nutzen.";

        public override string MAP_EDGES => "Map-Ecken";
        public override string MAP_LIMIT => "Map-Limit";
        public override string MAP_LIMIT_MIN_MAX => "Die Anzahl der Team-Limit-Positionen muss 0, min. {1} oder max. {2} sein.";
        public override string MAP_MAX_PLAYERS_MIN_MAX => "Die Max-Spieler Anzahl muss min. {1} und max. {2} sein.";
        public override string MAP_MIN_PLAYERS_MIN_MAX => "Die Min-Spieler Anzahl muss min. {1} und max. {2} sein.";
        public override string MAP_NAME_ALREADY_USED => "Der Map-Name ist schon in Benutzung.<br>Bitte ändere den Namen und nutze 'Prüfe' erneut.";
        public override string MAP_NAME_ATLEAST_CHARS => "Der Map-Name muss min. {1} Zeichen lang sein.";
        public override string MAP_TEAM_SPAWNS_MIN_PER_TEAM => "Die min. Anzahl an Team-Spawn muss min. {1} pro Team sein.";
        public override string MAX_PLAYERS => "Max-Spieler:";
        public override string MIN_PLAYERS => "Min-Spieler:";
        public override string NORMAL => "Normal";
        public override string REMOVE => "Entfernen";
        public override string SEND => "Senden";
        public override string TEAM => "Team";
        public override string TEAM_SPAWNS => "Team-Spawns";

        #endregion Public Properties
    }
}
