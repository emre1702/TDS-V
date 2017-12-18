namespace TDS.server.instance.damagesys {

	using System.Collections.Generic;
	using GTANetworkAPI;
	using lobby;

	internal partial class Damagesys : Script {

		private readonly FightLobby lobby;

		public Damagesys () {
		    Event.OnClientEventTrigger += OnPlayerHitOtherPlayer;
            Event.OnPlayerDeath += OnPlayerDeath;
		}

		public Damagesys ( FightLobby lobby, Dictionary<WeaponHash, int> customDamage = null,
							Dictionary<WeaponHash, float> customHeadMult = null ) {
			this.lobby = lobby;
			customDamageDictionary = customDamage ?? new Dictionary<WeaponHash, int> ();
			customHeadMultiplicator = customHeadMult ?? new Dictionary<WeaponHash, float> ();
		}

		public void EmptyDamagesysData () {
			AllHitters.Clear ();
			LastHitterDictionary.Clear ();
			PlayerDamage.Clear ();
			PlayerKills.Clear ();
			PlayerAssists.Clear ();
			PlayerSpree.Clear ();
		}
	}

}
