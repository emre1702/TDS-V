using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;

namespace Class {
	partial class Damagesys {

		public static void DamagesysOnStart ( API api ) {
			api.onClientEventTrigger += OnPlayerHitOtherPlayer;
			api.onPlayerDeath += OnPlayerDeath;
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