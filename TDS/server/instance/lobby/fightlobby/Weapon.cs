namespace TDS.server.instance.lobby {

	using System.Collections.Generic;
	using damagesys;
	using GTANetworkAPI;

	partial class FightLobby {

		private readonly List<WeaponHash> weapons = new List<WeaponHash> ();
		private readonly List<int> weaponsAmmo = new List<int> ();
		public Damagesys DmgSys;

		public void AddWeapon ( WeaponHash weapon, int ammo ) {
			weapons.Add ( weapon );
			weaponsAmmo.Add ( ammo );
		}

		public void GivePlayerWeapons ( Client player ) {
			player.RemoveAllWeapons ();
			for ( int i = 0; i < this.weapons.Count; i++ ) {
				player.GiveWeapon ( this.weapons[i], this.weaponsAmmo[i] );
			}
		}
	}

}
