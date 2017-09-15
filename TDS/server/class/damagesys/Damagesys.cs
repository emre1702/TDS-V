using System.Collections.Concurrent;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;

namespace Class {
	partial class Damagesys {

		public static void DamagesysOnStart ( API api ) {
			api.onClientEventTrigger += OnPlayerHitOtherPlayer;
			api.onPlayerDeath += OnPlayerDeath;
		}

		public Damagesys ( bool notusedvariable, ConcurrentDictionary<int, int> customdamage = null, ConcurrentDictionary<int, double> customheadmult = null ) {
			if ( customdamage == null )
				this.customDamageDictionary = new ConcurrentDictionary<int, int> ();
			else
				this.customDamageDictionary = customdamage;
			if ( customheadmult == null )
				this.customHeadMultiplicator = new ConcurrentDictionary<int, double> ();
			else
				this.customHeadMultiplicator = customheadmult;
		}

		public void EmptyDamagesysData ( ) {
			this.allHitters = new ConcurrentDictionary<Client, ConcurrentDictionary<Client, int>> ();
			this.lastHitterDictionary = new ConcurrentDictionary<Client, Client> ();
			this.playerDamage = new ConcurrentDictionary<Client, int> ();
			this.playerKills = new ConcurrentDictionary<Client, int> ();
			this.playerAssists = new ConcurrentDictionary<Client, int> ();
			this.playerSpree = new Dictionary<Client, int> ();
		}
	}
}