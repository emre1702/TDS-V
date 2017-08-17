using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

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

		public void EmptyDamagesysData ( ) {
			this.allHitters = new Dictionary<Client, Dictionary<Client, int>> ();
			this.lastHitterDictionary = new Dictionary<Client, Client> ();
			this.playerDamage = new Dictionary<Client, int> ();
			this.playerKills = new Dictionary<Client, int> ();
			this.playerAssists = new Dictionary<Client, int> ();
		}
	}
}