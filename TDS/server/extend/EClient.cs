namespace TDS.server.extend {

	using System.Collections.Concurrent;
	using GTANetworkAPI;
	using instance.player;

	internal static class EClient {

		private static ConcurrentDictionary<Client, Character> characterDictionary =
			new ConcurrentDictionary<Client, Character> ();

		public static Character GetChar ( this Client player ) {
			if ( characterDictionary.ContainsKey ( player ) ) {
				Character character = characterDictionary[player];
				return character;
			} else {
				Character character = new Character ( false );
				characterDictionary.TryAdd ( player, character );
				return character;
			}
		}

		public static bool IsAdminLevel ( this Client player, uint adminlvl, bool ownercando = false, bool vipcando = false ) {
			if ( player.Exists ) {
				Character character = player.GetChar ();
				if ( character.AdminLvl >= adminlvl || ownercando && character.IsLobbyOwner || vipcando && character.IsVIP ) {
					return true;
				}
			}
			return false;
		}

		public static uint AddHPArmor ( this Client player, uint hparmor ) {
			uint resthparmor = hparmor;
			if ( player.Health + resthparmor <= 100 ) {
				player.Health += (int) resthparmor;
				resthparmor = 0;
			} else {
				resthparmor -= 100 - (uint) player.Health;
				player.Health = 100;
				if ( player.Armor + resthparmor <= 100 ) {
					player.Armor += (int) resthparmor;
					resthparmor = 0;
				} else {
					resthparmor -= 100 - (uint) player.Armor;
					player.Armor = 100;
				}
			}
			return resthparmor;
		}
	}
}
