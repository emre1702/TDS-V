using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace Class {
	partial class Lobby {

		// BOMB DATA: 
		// name: prop_bomb_01_s
		// id: 1764669601

		// BOMB PLACE WHITE:
		// name: prop_mp_placement_med
		// id: -51423166

		// BOMB PLACE RED:
		// name: prop_mp_cant_place_med
		// id: -263709501

		private List<Object> bombPlantPlaces = new List<Object> ();
		private Object bomb;

		private void BombMapChose ( ) {
			for ( int i = 0; i < this.currentMap.bombPlantPlacesPos.Count; i++ ) {
				Object place = API.shared.createObject ( -51423166, this.currentMap.bombPlantPlacesPos[i], this.currentMap.bombPlantPlacesRot[i], this.dimension );
				this.bombPlantPlaces.Add ( place );
			}
			this.bomb = API.shared.createObject ( 1764669601, this.currentMap.bombPlantPlacesPos[0], this.currentMap.bombPlantPlacesRot[0], this.dimension );
		}

		private void GiveBombToRandomTerrorist ( ) {
			int amount = this.players[1].Count;
			if ( amount > 0 ) {
				int rnd = Manager.Utility.rnd.Next ( amount );
				Client player = this.players[1][rnd];
				this.bomb.collisionless = true;
				this.bomb.attachTo ( player, "SKEL_Spine_Root", new Vector3 (), new Vector3 () );
				// give it to the hand if he got no weapon there - on weapon switch //
			}
		}

		private void StartRoundBomb ( ) {
			this.SendAllPlayerLangNotification ( "round_mission_bomb_spectator", 0 );
			this.SendAllPlayerLangNotification ( "round_mission_bomb_good", 1 );
			this.SendAllPlayerLangNotification ( "round_mission_bomb_bad", 2 );
		}

		private void StopRoundBomb ( ) {
			for ( int i = 0; i < this.bombPlantPlaces.Count; i++ ) {
				this.bombPlantPlaces[i].delete ();
			}
			if ( this.bomb != null ) {
				bomb.delete ();
				this.bomb = null;
			}
			this.bombPlantPlaces = new List<Object> ();
		}
	}
}