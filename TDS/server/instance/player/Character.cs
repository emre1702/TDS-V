namespace TDS.server.instance.player {

    using GTANetworkAPI;
    using lobby;
    using Newtonsoft.Json;
    using TDS.server.enums;
    using TDS.server.manager.database;

    public class LobbyDeathmatchStats {
        public uint Kills = 0;
        public uint Assists = 0;
        public uint Deaths = 0;
        public uint Damage = 0;
        public uint TotalKills = 0;
        public uint TotalAssists = 0;
        public uint TotalDeaths = 0;
        public uint TotalDamage = 0;
    }

    public class Character {
        public Client Player;
		public uint UID;
		public uint AdminLvl = 0;
		public uint DonatorLvl = 0;
		public uint Playtime = 0;
		public uint Money = 0;
        public LobbyDeathmatchStats ArenaStats;
        public LobbyDeathmatchStats TempStats;
        public LobbyDeathmatchStats CurrentStats;
        public int Team = 0;
        public uint Gang = 0;
		public uint Lifes = 0;
		public Language Language = Language.ENGLISH;
        public Lobby Lobby = manager.lobby.MainMenu.TheLobby;
		public Character Spectating;
		public bool LoggedIn;
		public bool IsLobbyOwner = false;
		public bool IsVIP = false;

		public bool HitsoundOn = true;

		public Character ( Client player, bool loggedin = true ) {
            Player = player;
			LoggedIn = loggedin;
		}

        public void GiveMoney ( uint money ) {
            Money += money;
            NAPI.ClientEvent.TriggerClientEvent ( Player, "onClientMoneyChange", Money );
        }

        public void GiveKill () {
            ++CurrentStats.Kills;
            ++CurrentStats.TotalKills;
        }

        public void GiveAssist ( ) {
            ++CurrentStats.Assists;
            ++CurrentStats.TotalAssists;
        }

        public void GiveDeath ( ) {
            ++CurrentStats.Deaths;
            ++CurrentStats.TotalDeaths;
        }

        public void GiveDamage ( uint thedmg ) {
            CurrentStats.Damage += thedmg;
            CurrentStats.TotalDeaths += thedmg;
        }

        public void SaveData () {
            if ( LoggedIn ) {
                Database.Exec ( $"UPDATE player SET playtime = {Playtime}, money = {Money} WHERE uid = {UID}" );
                Database.Exec ( $"UPDATE playerarenastats SET arenakills = {ArenaStats.Kills}, arenaassists = {ArenaStats.Assists}, arenadeaths = {ArenaStats.Deaths}" +
                    $", arenadamage = {ArenaStats.Damage}, arenatotalkills = {ArenaStats.TotalKills}, arenatotalassists = {ArenaStats.TotalAssists}, arenatotaldeaths = {ArenaStats.TotalDeaths}" +
                    $", arenatotaldamage = {ArenaStats.TotalDamage} WHERE uid = {UID}" );
            }
        }

    }

}
