namespace TDS.server.instance.lobby {

	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using GTANetworkAPI;
	using manager.utility;
    using TDS.server.enums;
    using utility;

	partial class Arena {

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
		private Timer bombDetonateTimer,
		              bombPlantTimer,
	                  bombDefuseTimer;
		private Client planter;
		private Blip plantBlip;
		private Marker bombTakeMarker;

		private void BombMapChose () {
			foreach ( Vector3 bombplace in currentMap.BombPlantPlaces ) {
				Object place = NAPI.Object.CreateObject ( -51423166, bombplace, new Vector3 (), Dimension );
				bombPlantPlaces.Add ( place );
				Blip blip = NAPI.Blip.CreateBlip ( bombplace, Dimension );
				blip.Sprite = 433;
				bombPlantBlips.Add ( blip );
			}
			bomb = NAPI.Object.CreateObject ( 1764669601, currentMap.BombPlantPlaces[0], new Vector3 (), Dimension );
		}

		private void GiveBombToRandomTerrorist () {
			int amount = Players[terroristTeamID].Count;
			if ( amount > 0 ) {
				int rnd = Utility.Rnd.Next ( amount );
				Client player = NAPI.Player.GetPlayerFromHandle ( Players[terroristTeamID][rnd] );
				if ( player.CurrentWeapon == WeaponHash.Unarmed )
					BombToHand ( player );
				else
					BombToBack ( player );
				NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerGotBomb", currentMap.BombPlantPlaces );
			}
		}

		private void SendBombPlantInfos ( Client player ) {
			player.SendLangMessage ( "plant_info" );
		}

		private void SendBombDefuseInfos () {
			SendAllPlayerLangMessage ( "defuse_info_1", counterTerroristTeamID );
			SendAllPlayerLangMessage ( "defuse_info_2", counterTerroristTeamID );
		}

		private void BombToHand ( Client player ) {
			bomb.Detach ();
			bomb.Collisionless = true;
			bomb.AttachTo ( player, "SKEL_R_Finger01", new Vector3 ( 0.1, 0, 0 ), new Vector3 () );
			if ( bombAtPlayer != player )
				SendBombPlantInfos ( player );
			bombAtPlayer = player;
		}

		private void BombToBack ( Client player ) {
			bomb.Detach ();
			bomb.Collisionless = true;
			bomb.AttachTo ( player, "SKEL_Pelvis", new Vector3 ( 0, 0, 0.24 ), new Vector3 ( 270, 0, 0 ) );
			if ( bombAtPlayer != player )
				SendBombPlantInfos ( player );
			bombAtPlayer = player;
		}

		private void ToggleBombAtHand ( Client player, WeaponHash oldweapon ) {
			if ( player.CurrentWeapon == WeaponHash.Unarmed ) {
				BombToHand ( player );
			} else if ( oldweapon == WeaponHash.Unarmed ) {
				BombToBack ( player );
			}
		}

		private void StartRoundBomb () {
			SendAllPlayerLangNotification ( "round_mission_bomb_spectator", 0 );
			SendAllPlayerLangNotification ( "round_mission_bomb_good", 1 );
			SendAllPlayerLangNotification ( "round_mission_bomb_bad", 2 );

			if ( bombAtPlayer == null )
				GiveBombToRandomTerrorist ();
		}

		private void DetonateBomb () {
			NAPI.Explosion.CreateOwnedExplosion ( planter, ExplosionType.GrenadeL, bomb.Position, 200, Dimension );
			FuncIterateAllPlayers ( ( playerhandle, teamID ) => {
				DmgSys.LastHitterDictionary[playerhandle] = planter;
                Client player = NAPI.Player.GetPlayerFromHandle ( playerhandle );
                player.Kill ();
				NAPI.ClientEvent.TriggerClientEvent ( player, "onClientBombDetonated" );
			}, counterTerroristTeamID );
			// TERROR WON //
			if ( status == LobbyStatus.ROUND )
				EndRoundEarlier ();
		}

		private void PlantBomb ( Client player ) {
			if ( player.Exists ) {
				Vector3 playerpos = player.Position;
				for ( int i = 0; i < currentMap.BombPlantPlaces.Count; i++ ) {
                    if ( playerpos.DistanceTo ( currentMap.BombPlantPlaces[i] ) <= 5 ) {
                        NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerPlantedBomb" );
                        bomb.Detach ();
                        bomb.Position = new Vector3 ( playerpos.X, playerpos.Y, playerpos.Z - 0.9 );
                        bomb.Rotation = new Vector3 ( 270, 0, 0 );
                        bombPlantPlaces[i].Delete ();
                        bombPlantPlaces[i] =
                            NAPI.Object.CreateObject ( -263709501, currentMap.BombPlantPlaces[i], new Vector3 (), Dimension );
                        bombPlantBlips[i].Color = 49;
                        //bombPlantBlips[i].Flashing = true;
                        bombAtPlayer = null;
                        planter = player;
                        SendAllPlayerLangNotification ( "bomb_planted" );
                        bombDetonateTimer = Timer.SetTimer ( DetonateBomb, bombDetonateTime );
                        FuncIterateAllPlayers ( ( targethandle, teamID ) => {
                            Client target = NAPI.Player.GetPlayerFromHandle ( targethandle );
                            target.TriggerEvent ( "onClientBombPlanted", playerpos, teamID == counterTerroristTeamID );
                        } );                                                                                           
						SendBombDefuseInfos ();
						break;
					}
				}
				player.StopAnimation ();
			}
		}

		private void DefuseBomb ( Client player ) {
			if ( player.Exists ) {
				Vector3 playerpos = player.Position;
				if ( playerpos.DistanceTo ( bomb.Position ) <= 2 ) {
					FuncIterateAllPlayers ( ( targethandle, teamID ) => {
						DmgSys.LastHitterDictionary[targethandle] = player;
                        Client target = NAPI.Player.GetPlayerFromHandle ( targethandle );
                        target.Kill ();
					}, terroristTeamID );
					// COUNTER-TERROR WON //
					if ( status == LobbyStatus.ROUND )
						EndRoundEarlier ();
				}
				player.StopAnimation ();
			}
		}

		public void StartBombPlanting ( Client player ) {
			if ( bomb != null ) {
                if ( status == LobbyStatus.ROUND ) {
					if ( bombDetonateTimer == null ) {
						if ( bombPlantTimer != null ) {
							bombPlantTimer.Kill ();
							bombPlantTimer = null;
						}
						if ( !player.Dead ) {
							if ( player.CurrentWeapon == WeaponHash.Unarmed ) {
								player.PlayAnimation ( "misstrevor2ig_7", "plant_bomb", (int) ( Utility.AnimationFlags.Loop ) );
								bombPlantTimer = Timer.SetTimer ( () => PlantBomb ( player ), bombPlantTime );
							}
						}
					}
				}
			}
		}

        public void StopBombPlanting ( Client player ) {
			if ( bombPlantTimer != null ) {
				bombPlantTimer.Kill ();
				bombPlantTimer = null;
			}
			player.StopAnimation ();
		}

        public void StartBombDefusing ( Client player ) {
			if ( bomb != null ) {
                if ( status == LobbyStatus.ROUND ) {
					if ( bombDetonateTimer != null ) {
						if ( bombDefuseTimer != null ) {
							bombDefuseTimer.Kill ();
							bombDefuseTimer = null;
						}
						if ( !player.Dead ) {
							if ( player.CurrentWeapon == WeaponHash.Unarmed ) {
								if ( bombAtPlayer == null ) {
									player.PlayAnimation ( "misstrevor2ig_7", "plant_bomb", (int) ( Utility.AnimationFlags.Loop ) );
									bombDefuseTimer = Timer.SetTimer ( () => DefuseBomb ( player ), bombDefuseTime );
								}
							}
						}
					}
				}
			}
		}

        public void StopBombDefusing ( Client player ) {
            if ( bombDefuseTimer != null ) {
                bombDefuseTimer.Kill ();
                bombDefuseTimer = null;
            }
            player.StopAnimation ();
        }

        private void DropBomb () {
			bomb.Detach ();
			bomb.FreezePosition = true;
			bomb.Position = bombAtPlayer.Position;
			bombAtPlayer = null;
			bombTakeMarker = NAPI.Marker.CreateMarker ( 0, bomb.Position, new Vector3 (), new Vector3 (), 1,
														new Color ( 180, 0, 0, 180 ), true, Dimension );
			ColShape bombtakecol = NAPI.ColShape.CreateSphereColShape ( bomb.Position, 2 );
			lobbyBombTakeCol[this] = bombtakecol;
		}

		private void TakeBomb ( Client player ) {
			if ( player.CurrentWeapon == WeaponHash.Unarmed )
				BombToHand ( player );
			else
				BombToBack ( player );
			bomb.FreezePosition = false;
			bombTakeMarker.Delete ();
			bombTakeMarker = null;
			lobbyBombTakeCol.TryRemove ( this, out ColShape col  );
			NAPI.ColShape.DeleteColShape ( col );
		}

    
		private void StopRoundBombAtRoundEnd () {
			if ( bombPlantTimer != null ) {
				bombPlantTimer.Kill ();
				bombPlantTimer = null;
			}
			if ( bombDefuseTimer != null ) {
				bombDefuseTimer.Kill ();
				bombDefuseTimer = null;
			}
			if ( bombDetonateTimer != null ) {
				bombDetonateTimer.Kill ();
				bombDetonateTimer = null;
			}
			if ( lobbyBombTakeCol.ContainsKey ( this ) ) {
				lobbyBombTakeCol.TryRemove ( this, out ColShape col );
				NAPI.ColShape.DeleteColShape ( col );
				bombTakeMarker.Delete ();
				bombTakeMarker = null;
			}
		}

		private void StopRoundBomb () {
			foreach ( Object place in bombPlantPlaces ) {
				place.Delete ();
			}
			foreach ( Blip blip in bombPlantBlips ) {
				blip.Delete ();
			}
			if ( bomb != null ) {
				bomb.Delete ();
				bomb = null;
			}
			bombPlantPlaces = new List<Object> ();
			bombPlantBlips = new List<Blip> ();
			bombAtPlayer = null;
			planter = null;
			if ( plantBlip != null ) {
				plantBlip.Delete ();
				plantBlip = null;
			}
		}
	}

}
