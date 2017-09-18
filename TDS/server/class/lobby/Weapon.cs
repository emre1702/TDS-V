using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;

namespace Class {
	partial class Lobby {

		private List<WeaponHash> weapons = new List<WeaponHash> ();
		private List<int> weaponsAmmo = new List<int> ();
		public Damagesys damageSys;

		public void AddWeapon ( WeaponHash weapon, int ammo ) {
			this.weapons.Add ( weapon );
			this.weaponsAmmo.Add ( ammo );
		}

		private void GivePlayerWeapons ( Client player ) {
			player.removeAllWeapons ();
			for ( int i = 0; i < this.weapons.Count; i++ ) {
				player.giveWeapon ( this.weapons[i], this.weaponsAmmo[i], false, true );
			}
		}
	}
}