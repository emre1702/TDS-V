namespace TDS.server.instance.lobby {

	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using manager.utility;
	using utility;

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

		private static readonly ConcurrentDictionary<Lobby, ColShape> lobbyBombTakeCol = new ConcurrentDictionary<Lobby, ColShape> ();
		private const int counterTerroristTeamID = 1;
		private const int terroristTeamID = 2;

		private readonly uint bombDetonateTime = 45000;
		private readonly uint bombDefuseTime = 8000;
		private readonly uint bombPlantTime = 3000;

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

		private void BombMapChose () {
			foreach ( Vector3 bombplace in this.currentMap.BombPlantPlaces ) {
				Object place = this.API.CreateObject ( -51423166, bombplace, new Vector3 (), this.dimension );
				this.bombPlantPlaces.Add ( place );
				Blip blip = this.API.CreateBlip ( bombplace, this.dimension );
				blip.Sprite = 433;
				this.bombPlantBlips.Add ( blip );
			}
			this.bomb = this.API.CreateObject ( 1764669601, this.currentMap.BombPlantPlaces[0], new Vector3 (), this.dimension );
		}

		private void GiveBombToRandomTerrorist () {
			int amount = this.Players[terroristTeamID].Count;
			if ( amount > 0 ) {
				int rnd = Utility.Rnd.Next ( amount );
				Client player = this.Players[terroristTeamID][rnd];
				if ( player.CurrentWeapon == WeaponHash.Unarmed )
					this.BombToHand ( player );
				else
					this.BombToBack ( player );
				player.TriggerEvent ( "onClientPlayerGotBomb", this.currentMap.BombPlantPlaces );
			}
		}

		private void SendBombPlantInfos ( Client player ) {
			player.SendLangMessage ( "plant_info" );
		}

		private void SendBombDefuseInfos () {
			this.SendAllPlayerLangMessage ( "defuse_info_1", counterTerroristTeamID );
			this.SendAllPlayerLangMessage ( "defuse_info_2", counterTerroristTeamID );
		}

		private void BombToHand ( Client player ) {
			this.bomb.Detach ();
			this.bomb.Collisionless = true;
			this.bomb.AttachTo ( player, "SKEL_R_Finger01", new Vector3 ( 0.1, 0, 0 ), new Vector3 () );
			if ( this.bombAtPlayer != player )
				this.SendBombPlantInfos ( player );
			this.bombAtPlayer = player;
		}

		private void BombToBack ( Client player ) {
			this.bomb.Detach ();
			this.bomb.Collisionless = true;
			this.bomb.AttachTo ( player, "SKEL_Pelvis", new Vector3 ( 0, 0, 0.24 ), new Vector3 ( 270, 0, 0 ) );
			if ( this.bombAtPlayer != player )
				this.SendBombPlantInfos ( player );
			this.bombAtPlayer = player;
		}

		private void ToggleBombAtHand ( Client player, WeaponHash oldweapon ) {
			if ( player.CurrentWeapon == WeaponHash.Unarmed ) {
				this.BombToHand ( player );
			} else if ( oldweapon == WeaponHash.Unarmed ) {
				this.BombToBack ( player );
			}
		}

		private void StartRoundBomb () {
			this.SendAllPlayerLangNotification ( "round_mission_bomb_spectator", 0 );
			this.SendAllPlayerLangNotification ( "round_mission_bomb_good", 1 );
			this.SendAllPlayerLangNotification ( "round_mission_bomb_bad", 2 );

			if ( this.bombAtPlayer == null )
				this.GiveBombToRandomTerrorist ();
		}

		private void DetonateBomb () {
			this.API.CreateOwnedExplosion ( this.planter, ExplosionType.GrenadeL, this.bomb.Position, 200, this.dimension );
			this.FuncIterateAllPlayers ( ( player, teamID ) => {
				this.DmgSys.LastHitterDictionary[player] = this.planter;
				player.Kill ();
				player.TriggerEvent ( "onClientBombDetonated" );
			}, counterTerroristTeamID );
			// TERROR WON //
			if ( this.status == "round" )
				this.EndRoundEarlier ();
		}

		private void PlantBomb ( Client player ) {
			if ( player.Exists ) {
				Vector3 playerpos = player.Position;
				for ( int i = 0; i < this.currentMap.BombPlantPlaces.Count; i++ ) {
					if ( playerpos.DistanceTo ( this.currentMap.BombPlantPlaces[i] ) <= 5 ) {
						player.TriggerEvent ( "onClientPlayerPlantedBomb" );
						this.bomb.Detach ();
						this.bomb.Position = new Vector3 ( playerpos.X, playerpos.Y, playerpos.Z - 0.9 );
						this.bomb.Rotation = new Vector3 ( 270, 0, 0 );
						this.bombPlantPlaces[i].Delete ();
						this.bombPlantPlaces[i] =
							this.API.CreateObject ( -263709501, this.currentMap.BombPlantPlaces[i], new Vector3 (), this.dimension );
						this.bombPlantBlips[i].Color = 49;
						//this.bombPlantBlips[i].Flashing = true;
						this.bombAtPlayer = null;
						this.planter = player;
						this.SendAllPlayerLangNotification ( "bomb_planted" );
						this.bombDetonateTimer = Timer.SetTimer ( this.DetonateBomb, this.bombDetonateTime );
						this.FuncIterateAllPlayers ( ( target, teamID ) =>
														target.TriggerEvent ( "onClientBombPlanted", playerpos, teamID == counterTerroristTeamID ) );
						this.SendBombDefuseInfos ();
						break;
					}
				}
				player.StopAnimation ();
			}
		}

		private void DefuseBomb ( Client player ) {
			if ( player.Exists ) {
				Vector3 playerpos = player.Position;
				if ( playerpos.DistanceTo ( this.bomb.Position ) <= 2 ) {
					this.FuncIterateAllPlayers ( ( target, teamID ) => {
						this.DmgSys.LastHitterDictionary[target] = player;
						target.Kill ();
					}, terroristTeamID );
					// COUNTER-TERROR WON //
					if ( this.status == "round" )
						this.EndRoundEarlier ();
				}
				player.StopAnimation ();
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
						if ( !player.Dead ) {
							if ( player.CurrentWeapon == WeaponHash.Unarmed ) {
								player.PlayAnimation ( "misstrevor2ig_7", "plant_bomb", (int) ( Utility.AnimationFlags.Loop ) );
								this.bombPlantTimer = Timer.SetTimer ( () => this.PlantBomb ( player ), this.bombPlantTime );
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
			player.StopAnimation ();
		}

		private void StartBombDefusing ( Client player ) {
			if ( this.bomb != null ) {
				if ( this.status == "round" ) {
					if ( this.bombDetonateTimer != null ) {
						if ( this.bombDefuseTimer != null ) {
							this.bombDefuseTimer.Kill ();
							this.bombDefuseTimer = null;
						}
						if ( !player.Dead ) {
							if ( player.CurrentWeapon == WeaponHash.Unarmed ) {
								if ( this.bombAtPlayer == null ) {
									player.PlayAnimation ( "misstrevor2ig_7", "plant_bomb", (int) ( Utility.AnimationFlags.Loop ) );
									this.bombDefuseTimer = Timer.SetTimer ( () => this.DefuseBomb ( player ), this.bombDefuseTime );
								}
							}
						}
					}
				}
			}
		}

		private void DropBomb () {
			this.bomb.Detach ();
			this.bomb.FreezePosition = true;
			this.bomb.Position = this.bombAtPlayer.Position;
			this.bombAtPlayer = null;
			this.bombTakeMarker = this.API.CreateMarker ( 0, this.bomb.Position, new Vector3 (), new Vector3 (), 1,
														new Color ( 180, 0, 0, 180 ), true, true, this.dimension );
			ColShape bombtakecol = this.API.CreateSphereColShape ( this.bomb.Position, 2 );
			lobbyBombTakeCol[this] = bombtakecol;
		}

		private void TakeBomb ( Client player ) {
			if ( player.CurrentWeapon == WeaponHash.Unarmed )
				this.BombToHand ( player );
			else
				this.BombToBack ( player );
			this.bomb.FreezePosition = false;
			this.bombTakeMarker.Delete ();
			this.bombTakeMarker = null;
			lobbyBombTakeCol.TryRemove ( this, out ColShape col  );
			this.API.DeleteColShape ( col );
		}

		private void StopBombDefusing ( Client player ) {
			if ( this.bombDefuseTimer != null ) {
				this.bombDefuseTimer.Kill ();
				this.bombDefuseTimer = null;
			}
			player.StopAnimation ();
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
				lobbyBombTakeCol.TryRemove ( this, out ColShape col );
				this.API.DeleteColShape ( col );
				this.bombTakeMarker.Delete ();
				this.bombTakeMarker = null;
			}
		}

		private void StopRoundBomb () {
			foreach ( Object place in this.bombPlantPlaces ) {
				place.Delete ();
			}
			foreach ( Blip blip in this.bombPlantBlips ) {
				blip.Delete ();
			}
			if ( this.bomb != null ) {
				this.bomb.Delete ();
				this.bomb = null;
			}
			this.bombPlantPlaces = new List<Object> ();
			this.bombPlantBlips = new List<Blip> ();
			this.bombAtPlayer = null;
			this.planter = null;
			if ( this.plantBlip != null ) {
				this.plantBlip.Delete ();
				this.plantBlip = null;
			}
		}
	}

}
