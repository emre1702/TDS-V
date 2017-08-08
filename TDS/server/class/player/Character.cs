using System;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;

namespace Class {
	class Character {
		public int uID;
		public int adminLvl = 0;
		public int donatorLvl = 0;
		public int playtime = 0;
		public int kills = 0;
		public int assists = 0;
		public int deaths = 0;
		public int damage = 0;
		public int team = 0;
		public int lifes = 0;
		public string language = "english";
		public Lobby lobby = Manager.MainMenu.lobby;
		public Client spectating;
		public bool loggedIn = true;
		public bool isLobbyOwner = false;
		public bool isVIP = false;

		public bool hitsoundOn = true;

		public Character ( bool loggedin = true ) {
			this.loggedIn = loggedin;
		}

	}
}

