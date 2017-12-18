using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.map;
using TDS.server.instance.player;

namespace TDS.server.instance.lobby {

    partial class Arena {

        public uint Lifes = 1;

        public override void OnPlayerDeath ( Client player, Client killer, uint weapon, Character character ) {
            if ( character.Lifes > 0 ) {
                base.OnPlayerDeath ( player, killer, weapon, character );

                if ( bombAtPlayer == player ) {
                    DropBomb ();
                }
                if ( character.Lifes == 0 ) {
                    RemovePlayerFromAlive ( player, character );
                }
            }
        }

        public override void OnPlayerSpawn ( Client player ) {
            base.OnPlayerSpawn ( player );

            Character character = player.GetChar ();
            Lobby lobby = character.Lobby;

            if ( character.Lifes > 0 )
                RespawnPlayerInRound ( player );
            else
                RespawnPlayerInSpectateMode ( player );

           /* } else { TODO Mainmenu
                player.Position = lobby.spawnpoint.Around ( 5 );
                player.Freeze ( true );
            }  */
        }

        private void SetPlayerReadyForRound ( Client player, uint teamID ) {
            player.Armor = (int) Armor;
            player.Health = (int) Health;
            Spectate ( player, player );
            if ( teamID > 0 ) {
                Vector3[] spawndata = this.GetMapRandomSpawnData ( teamID );
                player.Position = spawndata[0];
                player.Rotation = spawndata[1];
            }
            player.Freeze ( true );
            GivePlayerWeapons ( player );
        }

        public override void RemovePlayer ( Client player ) {
            base.RemovePlayer ( player );

            Character character = player.GetChar ();
            uint teamID = character.Team;

            if ( character.Lifes > 0 ) {
                DmgSys.CheckLastHitter ( player, character, out Client killer );
                DeathInfoSync ( player, teamID, killer, (uint) WeaponHash.Unarmed );
                RemovePlayerFromAlive ( player, character );
            }
            DmgSys.PlayerSpree.Remove ( player );


            //if ( this.IsMapCreateLobby )                            TODO
            //    this.StopPlayerFreecam ( player, true );
        }

        private void RemovePlayerFromAlive ( Client player, Character chara = null ) {
            Character character = chara ?? player.GetChar ();
            int teamID = (int) character.Team;
            character.Lifes = 0;
            int aliveindex = this.alivePlayers[teamID].IndexOf ( player );
            this.PlayerCantBeSpectatedAnymore ( player, aliveindex, teamID );
            this.alivePlayers[teamID].RemoveAt ( aliveindex );
            if ( this.bombAtPlayer == player ) {
                this.DropBomb ();
            }
            CheckForEnoughAlive ();
        }

        public override void AddPlayer ( Client player, bool spectator = false ) {
            AddPlayerDefault ( player, spectator );

            string mapname = currentMap != null ? currentMap.Name : "unknown";
            player.TriggerEvent ( "onClientPlayerJoinLobby", spectator, mapname, Teams, teamColorsList,
                                countdownTime, roundTime, bombDetonateTime, bombPlantTime, bombDefuseTime,
                                RoundEndTime );

            if ( !spectator )
                AddPlayerAsPlayer ( player );

            SendPlayerRoundInfoOnJoin ( player );
        }



           /* if ( this.IsMapCreateLobby )        // TODO
                this.StartPlayerFreecam ( player );

            this.SendPlayerRoundInfoOnJoin ( player );
        } */


        private void AddPlayerAsPlayer ( Client player ) {
            Character character = player.GetChar ();
            uint teamID = GetTeamIDWithFewestMember ( Players );
            Players[(int) teamID].Add ( player );
            player.SetSkin ( teamSkins[(int) teamID] );
            character.Team = teamID;
            if ( countdownTimer != null && countdownTimer.IsRunning ) {
                SetPlayerReadyForRound ( player, teamID );
            } else {
                int teamsinround = GetTeamAmountStillInRound ();
                NAPI.Util.ConsoleOutput ( teamsinround + " teams still in round" );
                if ( teamsinround < 2 ) {
                    EndRoundEarlier ();
                    NAPI.Util.ConsoleOutput ( "End round earlier because of joined player" );
                } else {
                    RespawnPlayerInSpectateMode ( player );
                }
            }
        }

        public static void PlayerAmountInFightSync ( Client player, List<uint> amountinteam, List<uint> amountaliveinteam ) {
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerAmountInFightSync", amountinteam, true, amountaliveinteam );
        }

        private void SendPlayerRoundInfoOnJoin ( Client player ) {
            if ( currentMap != null ) {
                player.TriggerEvent ( "onClientMapChange", this.currentMap.MapLimits, this.currentMap.MapCenter );
            }

            SendPlayerAmountInFightInfo ( player );
            SyncMapVotingOnJoin ( player );


            int tick = Environment.TickCount;
            switch ( status ) {
                case LobbyStatus.COUNTDOWN:
                    Map map = currentMap;
                    if ( map != null )
                        player.TriggerEvent ( "onClientCountdownStart", map.Name, tick - startTick );
                    break;
                case LobbyStatus.ROUND:
                    player.TriggerEvent ( "onClientRoundStart", true, tick - startTick );
                    break;
            }
        }

    }
}
