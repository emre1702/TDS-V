using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared;

namespace Class {
	partial class Lobby : Script {

		private List<WeaponHash> weapons = new List<WeaponHash> ();
		private List<int> weaponsAmmo = new List<int> ();
		public Damagesys damageSys;

		public void AddWeapon ( WeaponHash weapon, int ammo ) {
			this.weapons.Add ( weapon );
			this.weaponsAmmo.Add ( ammo );
		}
	}
}