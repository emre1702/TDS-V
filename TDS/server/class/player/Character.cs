using System;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;

namespace Class {
	class Character {
		public int uID;
		public int adminLvl = 0;
		public int vipLvl = 0;
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

		public Character ( bool loggedin = true ) {
			this.loggedIn = loggedin;
		}

	}
}
static class PlayerCharacter {
	private static Dictionary<Client, Class.Character> characterdictionary = new Dictionary<Client, Class.Character> ();

	public static Class.Character GetChar ( this Client player ) {
		if ( characterdictionary.ContainsKey ( player ) ) {
			Class.Character character = characterdictionary[player];
			return character;
		} else {
			Class.Character character = new Class.Character ( false );
			characterdictionary[player] = character;
			return character;
		}
	}

	public static bool IsAdminLevel ( this Client player, int adminlvl, bool ownercando = false ) {
		if ( player.exists ) {
			Class.Character character = player.GetChar ();
			if ( character.adminLvl >= adminlvl || ownercando && character.isLobbyOwner ) {
				return true; 
			}
		}
		return false;
	}
}
