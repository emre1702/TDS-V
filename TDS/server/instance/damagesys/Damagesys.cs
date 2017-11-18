namespace TDS.server.instance.damagesys {

	using System.Collections.Generic;
	using GTANetworkAPI;
	using lobby;

	internal partial class Damagesys : Script {

		private Lobby lobby;

		public Damagesys () {
			API.OnClientEventTrigger += this.OnPlayerHitOtherPlayer;
			API.OnPlayerDeath += this.OnPlayerDeath;
		}

		public Damagesys ( Lobby lobby, Dictionary<int, int> customDamage = null,
							Dictionary<int, float> customHeadMult = null ) {
			this.lobby = lobby;
			this.customDamageDictionary = customDamage ?? new Dictionary<int, int> ();
			this.customHeadMultiplicator = customHeadMult ?? new Dictionary<int, float> ();
		}

		public void EmptyDamagesysData () {
			this.AllHitters.Clear ();
			this.LastHitterDictionary.Clear ();
			this.PlayerDamage.Clear ();
			this.PlayerKills.Clear ();
			this.PlayerAssists.Clear ();
			this.PlayerSpree.Clear ();
		}
	}

}
