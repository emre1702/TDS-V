using GTANetworkAPI;
using Newtonsoft.Json;
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
        }

        private void SetPlayerReadyForRound ( Client player, int teamID ) {
            player.Armor = (int) Armor;
            player.Health = (int) Health;
            Spectate ( player, player );
            if ( teamID > 0 ) {
                Vector3[] spawndata = GetMapRandomSpawnData ( teamID );
                player.Position = spawndata[0];
                player.Rotation = spawndata[1];
            }
            player.Freeze ( true );
            GivePlayerWeapons ( player );
        }

        public override void RemovePlayer ( Client player ) {
            base.RemovePlayer ( player );

            Character character = player.GetChar ();
            int teamID = character.Team;

            if ( character.Lifes > 0 ) {
                DmgSys.CheckLastHitter ( player, character, out Client killer );
                DeathInfoSync ( player, teamID, killer, (uint) WeaponHash.Unarmed );
                RemovePlayerFromAlive ( player, character );
            }
            DmgSys.PlayerSpree.Remove ( player );

            //if ( IsMapCreateLobby )                            TODO
            //    StopPlayerFreecam ( player, true );
        }

        private void RemovePlayerFromAlive ( Client player, Character chara = null ) {
            Character character = chara ?? player.GetChar ();
            int teamID = (int) character.Team;
            character.Lifes = 0;
            int aliveindex = alivePlayers[teamID].IndexOf ( player );
            PlayerCantBeSpectatedAnymore ( player, aliveindex, teamID );
            alivePlayers[teamID].RemoveAt ( aliveindex );
            if ( bombAtPlayer == player ) {
                DropBomb ();
            }
            CheckForEnoughAlive ();
        }

        public override void AddPlayer ( Client player, bool spectator = false ) {
            AddPlayerDefault ( player, spectator );

            string mapname = currentMap != null ? currentMap.SyncData.Name : "unknown";
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerJoinLobby", ID, spectator, mapname, JsonConvert.SerializeObject ( Teams ), JsonConvert.SerializeObject ( teamColorsList ), 
                                countdownTime, roundTime, bombDetonateTime, bombPlantTime, bombDefuseTime,
                                RoundEndTime, true );

            if ( !spectator )
                AddPlayerAsPlayer ( player );

            SendPlayerRoundInfoOnJoin ( player );
        }



           /* if ( IsMapCreateLobby )        // TODO
                StartPlayerFreecam ( player );

            SendPlayerRoundInfoOnJoin ( player );
        } */


        private void AddPlayerAsPlayer ( Client player ) {
            Character character = player.GetChar ();
            int teamID = GetTeamIDWithFewestMember ( ref Players );
            SetPlayerTeam ( player, teamID, character );
            if ( countdownTimer != null && countdownTimer.IsRunning ) {
                SetPlayerReadyForRound ( player, teamID );
            } else {
                int teamsinround = GetTeamAmountStillInRound ();
                NAPI.Util.ConsoleOutput ( teamsinround + " teams still in round" );
                if ( teamsinround < 2 ) {
                    EndRoundEarlier ( RoundEndReason.NEWPLAYER, player.Name );
                    NAPI.Util.ConsoleOutput ( "End round earlier because of joined player" );
                } else {
                    RespawnPlayerInSpectateMode ( player );
                }
            }
        }

        public static void PlayerAmountInFightSync ( Client player, List<uint> amountinteam, List<uint> amountaliveinteam ) {
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientPlayerAmountInFightSync", JsonConvert.SerializeObject ( amountinteam ), 1, JsonConvert.SerializeObject ( amountaliveinteam ) );
        }

        private void SendPlayerRoundInfoOnJoin ( Client player ) {
            if ( currentMap != null ) {
                List<Tuple<float, float>> maplimits = GetJsonSerializableList ( currentMap.MapLimits );
                NAPI.ClientEvent.TriggerClientEvent ( player, "onClientMapChange", currentMap.SyncData.Name, JsonConvert.SerializeObject ( maplimits ), currentMap.MapCenter.X, currentMap.MapCenter.Y, currentMap.MapCenter.Z );
            }

            SendPlayerAmountInFightInfo ( player );
            SyncMapVotingOnJoin ( player );


            int tick = Environment.TickCount;
            switch ( status ) {
                case LobbyStatus.COUNTDOWN:
                    Map map = currentMap;
                    if ( map != null )
                        NAPI.ClientEvent.TriggerClientEvent ( player, "onClientCountdownStart", tick - startTick );
                    break;
                case LobbyStatus.ROUND:
                    NAPI.ClientEvent.TriggerClientEvent ( player, "onClientRoundStart", 1, tick - startTick );
                    break;
            }
        }

    }
}
