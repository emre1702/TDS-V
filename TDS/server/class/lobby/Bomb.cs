using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
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

		private static Dictionary<Lobby, SphereColShape> lobbyBombTakeCol = new Dictionary<Lobby, SphereColShape> ();
		private static int counterTerroristTeamID = 1;
		private static int terroristTeamID = 2;

		private List<Object> bombPlantPlaces = new List<Object> ();
		private List<Blip> bombPlantBlips = new List<Blip> ();
		private Object bomb;
		private Client bombAtPlayer;
		private Timer bombDetonateTimer;
		private Timer bombPlantTimer;
		private Timer bombDefuseTimer;
		private Client planter;
		private Blip plantBlip;
		private Marker bombTakeMarker;

		private void BombMapChose ( ) {
			for ( int i = 0; i < this.currentMap.bombPlantPlaces.Count; i++ ) {
				Object place = API.shared.createObject ( -51423166, this.currentMap.bombPlantPlaces[i], new Vector3(), this.dimension );
				this.bombPlantPlaces.Add ( place );
				Blip blip = API.shared.createBlip ( this.currentMap.bombPlantPlaces[i], this.dimension );
				blip.sprite = 433;
				this.bombPlantBlips.Add ( blip );
			}
			this.bomb = API.shared.createObject ( 1764669601, this.currentMap.bombPlantPlaces[0], new Vector3(), this.dimension );
		}

		private void GiveBombToRandomTerrorist ( ) {
			int amount = this.players[terroristTeamID].Count;
			if ( amount > 0 ) {
				int rnd = Manager.Utility.rnd.Next ( amount );
				Client player = this.players[terroristTeamID][rnd];
				this.BombToBack ( player );
				player.triggerEvent ( "onClientPlayerGotBomb", this.currentMap.bombPlantPlaces );
			}
		}

		private void BombToHand ( Client player ) {
			this.bomb.detach ();
			this.bomb.collisionless = true;
			this.bomb.attachTo ( player, "SKEL_R_Finger01", new Vector3 ( 0.1, 0, 0 ), new Vector3 () );
			this.bombAtPlayer = player;
		}

		private void BombToBack ( Client player ) {
			this.bomb.detach ();
			this.bomb.collisionless = true;
			this.bomb.attachTo ( player, "SKEL_Pelvis", new Vector3 ( 0, 0, 0.24 ), new Vector3 ( 270, 0, 0 ) );
			this.bombAtPlayer = player;
		}

		private void ToggleBombAtHand ( Client player, WeaponHash oldweapon ) {
			if ( oldweapon == WeaponHash.Unarmed ) {
				this.BombToBack ( player );
			} else if ( player.currentWeapon == WeaponHash.Unarmed ) {
				this.BombToHand ( player );
			}
		}

		private void StartRoundBomb ( ) {
			this.SendAllPlayerLangNotification ( "round_mission_bomb_spectator", 0 );
			this.SendAllPlayerLangNotification ( "round_mission_bomb_good", 1 );
			this.SendAllPlayerLangNotification ( "round_mission_bomb_bad", 2 );

			if ( this.bombAtPlayer == null )
				this.GiveBombToRandomTerrorist ();
		}

		private void DetonateBomb () {
			API.shared.createOwnedExplosion ( this.planter, ExplosionType.GrenadeL, this.bomb.position, 200, this.dimension );
			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				this.damageSys.lastHitterDictionary[player] = this.planter;
				player.kill ();
			}, counterTerroristTeamID );
			// TERROR WON //
			if ( this.status == "round" )
				this.EndRoundEarlier ();
		}

		private void PlantBomb ( Client player ) {
			if ( player.exists ) {
				Vector3 playerpos = player.position;
				for ( int i = 0; i < this.currentMap.bombPlantPlaces.Count; i++ ) {
					if ( playerpos.DistanceTo ( this.currentMap.bombPlantPlaces[i] ) <= 5 ) {
						player.triggerEvent ( "onClientPlayerPlantedBomb" );
						this.SendAllPlayerEvent ( "onClientBombPlanted", counterTerroristTeamID, playerpos );
						this.bomb.detach ();
						this.bomb.position = new Vector3 ( playerpos.X, playerpos.Y, playerpos.Z - 0.9 );
						this.bomb.rotation = new Vector3 ( 270, 0, 0 );
						this.bombPlantPlaces[i].delete ();
						this.bombPlantPlaces[i] = API.shared.createObject ( -263709501, this.currentMap.bombPlantPlaces[i], new Vector3(), this.dimension );
						this.bombPlantBlips[i].color = 49;
						//API.shared.setBlipFlashing ( this.bombPlantBlips[i], true );
						this.bombAtPlayer = null;
						this.planter = player;
						this.SendAllPlayerLangNotification ( "bomb_planted" );
						this.bombDetonateTimer = Timer.SetTimer ( this.DetonateBomb, 45000, 1 );
						break;
					}
				}
				player.stopAnimation ();
			}
		}

		private void DefuseBomb ( Client player ) {
			if ( player.exists ) {
				Vector3 playerpos = player.position;
				if ( playerpos.DistanceTo ( this.bomb.position ) <= 2 ) {
					this.FuncIterateAllPlayers ( ( target, teamID ) => {
						this.damageSys.lastHitterDictionary[target] = player;
						target.kill ();
					}, terroristTeamID );
					// COUNTER-TERROR WON //
					if ( this.status == "round" )
						this.EndRoundEarlier ();
				}
				player.stopAnimation ();
			}
		}

		private void StartBombPlanting ( Client player ) {
			if ( this.bomb != null ) {
				if ( this.status == "round" ) {
					if ( this.bombDetonateTimer == null ) {
						if ( this.bombPlantTimer != null ) {
							this.bombPlantTimer.Kill ();
							this.bombPlantTimer = null;
						}
						if ( !player.dead ) {
							if ( player.currentWeapon == WeaponHash.Unarmed ) {
								player.playAnimation ( "misstrevor2ig_7", "plant_bomb", (int) ( Manager.Utility.AnimationFlags.Loop | Manager.Utility.AnimationFlags.Cancellable ) );
								this.bombPlantTimer = Timer.SetTimer ( ( ) => PlantBomb ( player ), 3000, 1 );
							}
						}
					}
				}
			}
		}

		private void StopBombPlanting ( Client player ) {
			if ( this.bombPlantTimer != null ) {
				this.bombPlantTimer.Kill ();
				this.bombPlantTimer = null;
			}
			player.stopAnimation ();
		}

		private void StartBombDefusing ( Client player ) {
			if ( this.bomb != null ) {
				if ( this.status == "playing" ) {
					if ( this.bombDetonateTimer != null ) {
						if ( this.bombDefuseTimer != null ) {
							this.bombDefuseTimer.Kill ();
							this.bombDefuseTimer = null;
						}
						if ( !player.dead ) {
							if ( player.currentWeapon == WeaponHash.Unarmed ) {
								if ( this.bombAtPlayer == null ) {
									player.playAnimation ( "misstrevor2ig_7", "plant_bomb", (int) ( Manager.Utility.AnimationFlags.Loop | Manager.Utility.AnimationFlags.Cancellable ) );
									this.bombDefuseTimer = Timer.SetTimer ( ( ) => DefuseBomb ( player ), 8000, 1 );
								}
							}
						}
					}
				}
			}
		}

		private void DropBomb () {
			this.bomb.detach ();
			this.bomb.freezePosition = true;
			this.bomb.position = this.bombAtPlayer.position;
			this.bombAtPlayer = null;
			this.bombTakeMarker = API.shared.createMarker ( 0, this.bomb.position, new Vector3 (), new Vector3 (), new Vector3 ( 1, 1, 1 ), 180, 180, 0, 0, this.dimension );
			SphereColShape bombtakecol = API.shared.createSphereColShape ( this.bomb.position, 2 );
			lobbyBombTakeCol[this] = bombtakecol;
		}

		private void TakeBomb ( Client player ) {
			this.bombAtPlayer = player;
			if ( player.currentWeapon == WeaponHash.Unarmed )
				this.BombToHand ( player );
			else
				this.BombToBack ( player );
			this.bomb.freezePosition = false;
			this.bombTakeMarker.delete ();
			this.bombTakeMarker = null;
			API.shared.deleteColShape ( lobbyBombTakeCol[this] );
			lobbyBombTakeCol.Remove ( this );
		}

		private void StopBombDefusing ( Client player ) {
			if ( this.bombDefuseTimer != null ) {
				this.bombDefuseTimer.Kill ();
				this.bombDefuseTimer = null;
			}
			player.stopAnimation ();
		}

		private void StopRoundBombAtRoundEnd () {
			if ( this.bombPlantTimer != null ) {
				this.bombPlantTimer.Kill ();
				this.bombPlantTimer = null;
			}
			if ( this.bombDefuseTimer != null ) {
				this.bombDefuseTimer.Kill ();
				this.bombDefuseTimer = null;
			}
			if ( this.bombDetonateTimer != null ) {
				this.bombDetonateTimer.Kill ();
				this.bombDetonateTimer = null;
			}
			if ( lobbyBombTakeCol.ContainsKey ( this ) ) {
				API.shared.deleteColShape ( lobbyBombTakeCol[this] );
				lobbyBombTakeCol.Remove ( this );
				this.bombTakeMarker.delete ();
				this.bombTakeMarker = null;
			}
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
			this.bombAtPlayer = null;
			this.planter = null;
			if ( this.plantBlip != null ) {
				this.plantBlip.delete ();
				this.plantBlip = null;
			}
		}
	}
}