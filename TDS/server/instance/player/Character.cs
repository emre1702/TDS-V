namespace TDS.server.instance.player {

    using GTANetworkAPI;
    using lobby;
    using Newtonsoft.Json;
    using TDS.server.enums;

    public class LobbyDeathmatchStats {
        public uint Kills = 0;
        public uint Assists = 0;
        public uint Deaths = 0;
        [JsonIgnore]
        public uint Damage = 0;
        [JsonIgnore]
        public uint TotalKills = 0;
        [JsonIgnore]
        public uint TotalAssists = 0;
        [JsonIgnore]
        public uint TotalDeaths = 0;
        [JsonIgnore]
        public uint TotalDamage = 0;
    }

    public class Character {
		public uint UID;
		public uint AdminLvl = 0;
		public uint DonatorLvl = 0;
		public uint Playtime = 0;
		public uint Money = 0;
        public LobbyDeathmatchStats ArenaStats;
        public LobbyDeathmatchStats CurrentStats;
        public int Team = 0;
		public uint Lifes = 0;
		public Language Language = Language.ENGLISH;
        public Lobby Lobby = manager.lobby.MainMenu.TheLobby;
		public Client Spectating;
		public bool LoggedIn;
		public bool IsLobbyOwner = false;
		public bool IsVIP = false;

		public bool HitsoundOn = true;

		public Character ( bool loggedin = true ) {
			LoggedIn = loggedin;
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

    }

}
