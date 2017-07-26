using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;

namespace Class {
	partial class Damagesys : Script {

		public Damagesys ( ) {
			API.onClientEventTrigger += this.OnPlayerHitOtherPlayer;
			API.onPlayerDeath += this.OnPlayerDeath;
		}

		public Damagesys ( bool notusedvariable, Dictionary<int, int> customdamage = null, Dictionary<int, double> customheadmult = null ) {
			if ( customdamage == null )
				this.customDamageDictionary = new Dictionary<int, int> ();
			else
				this.customDamageDictionary = customdamage;
			if ( customheadmult == null )
				this.customHeadMultiplicator = new Dictionary<int, double> ();
			else
				this.customHeadMultiplicator = customheadmult;
		}
	}
}