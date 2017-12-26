namespace TDS.server.instance.player {

    using GTANetworkAPI;
    using lobby;
    using TDS.server.enums;

    public class Character {
		public uint UID;
		public uint AdminLvl = 0;
		public uint DonatorLvl = 0;
		public uint Playtime = 0;
		public uint Money = 0;
		public uint Kills = 0;
		public uint Assists = 0;
		public uint Deaths = 0;
		public uint Damage = 0;
		public uint Team = 0;
		public uint Lifes = 0;
		public Language Language = Language.ENGLISH;
		public Lobby Lobby;     //TODO = Main menu
		public Client Spectating;
		public bool LoggedIn;
		public bool IsLobbyOwner = false;
		public bool IsVIP = false;

		public bool HitsoundOn = true;

		public Character ( bool loggedin = true ) {
			LoggedIn = loggedin;
		}

	}

}
