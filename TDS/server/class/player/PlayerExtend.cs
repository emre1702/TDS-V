using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;

static class PlayerExtend {
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

	public static bool IsAdminLevel ( this Client player, int adminlvl, bool ownercando = false, bool vipcando = false ) {
		if ( player.exists ) {
			Class.Character character = player.GetChar ();
			if ( character.adminLvl >= adminlvl || ownercando && character.isLobbyOwner || vipcando && character.isVIP ) {
				return true;
			}
		}
		return false;
	}
}