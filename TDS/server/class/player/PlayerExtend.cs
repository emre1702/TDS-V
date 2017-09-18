using System.Collections.Concurrent;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;

static class PlayerExtend {
	private static ConcurrentDictionary<Client, Class.Character> characterdictionary = new ConcurrentDictionary<Client, Class.Character> ();

	public static Class.Character GetChar ( this Client player ) {
		if ( characterdictionary.ContainsKey ( player ) ) {
			Class.Character character = characterdictionary[player];
			return character;
		} else {
			Class.Character character = new Class.Character ( false );
			characterdictionary.TryAdd ( player, character );
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

	public static int AddHPArmor ( this Client player, int hparmor ) {
		int resthparmor = hparmor;
		if ( player.health + resthparmor <= 100 ) {
			player.health += resthparmor;
			resthparmor = 0;
		} else {
			resthparmor -= 100 - player.health;
			player.health = 100;
			if ( player.armor + resthparmor <= 100 ) {
				player.armor += resthparmor;
				resthparmor = 0;
			} else {
				resthparmor -= 100 - player.armor;
				player.armor = 100;
			}
		}
		return resthparmor;
	}
}