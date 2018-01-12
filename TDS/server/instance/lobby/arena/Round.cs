using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.instance.utility;

namespace TDS.server.instance.lobby {

    partial class Arena {

        private Timer roundStartTimer,
                        countdownTimer,
                        roundEndTimer;
        private uint countdownTime = 5 * 1000;
        private uint roundTime = 4 * 60 * 1000;
        public uint RoundEndTime = 8 * 1000;
        private long startTick;
        private bool mixTeamsAfterRound = true;


        private int GetTeamAmountStillInRound ( int minalive = 1 ) {
            int amount = 0;
            for ( int i = 1; i < alivePlayers.Count; i++ )
                if ( alivePlayers[i].Count >= minalive )
                    amount++;
            return amount;
        }

        public void StartRoundGame ( ) {
            StartMapChoose ();
        }

        private static List<Tuple<float, float>> GetJsonSerializableList ( List<Vector3> list ) {
            List<Tuple<float, float>> newlist = new List<Tuple<float, float>> ();
            foreach ( Vector3 vector in list ) {
                newlist.Add ( new Tuple<float, float> ( vector.X, vector.Y ) );
            }
            return newlist;
        } 

        public void StartMapChoose ( ) {
            try {
                status = LobbyStatus.MAPCHOOSE;
                NAPI.Util.ConsoleOutput ( status.ToString () );
                if ( IsOfficial )
                    RewardAllPlayer ();
                DmgSys.EmptyDamagesysData ();

                map.Map oldMap = currentMap;
                currentMap = GetNextMap ();
                if ( oldMap != null && oldMap.SyncData.Type == MapType.BOMB )
                    StopRoundBomb ();
                if ( currentMap.SyncData.Type == MapType.BOMB )
                    BombMapChose ();
                CreateTeamSpawnBlips ();
                CreateMapLimitBlips ();
                if ( mixTeamsAfterRound )
                    MixTeams ();
                List <Tuple<float, float>> maplimits = GetJsonSerializableList ( currentMap.MapLimits );
                SendAllPlayerEvent ( "onClientMapChange", -1, JsonConvert.SerializeObject ( maplimits ), currentMap.MapCenter.X, currentMap.MapCenter.Y, currentMap.MapCenter.Z );

                roundStartTimer = Timer.SetTimer ( StartRoundCountdown, RoundEndTime / 2 );
            } catch ( Exception ex ) {
                NAPI.Util.ConsoleOutput ( "Error in StartMapChoose:" + ex.Message );
            }
        }

        private void StartRoundCountdown ( ) {
            status = LobbyStatus.COUNTDOWN;
            NAPI.Util.ConsoleOutput ( status.ToString () );
            spectatingMe = new Dictionary<NetHandle, List<NetHandle>> ();
            SetAllPlayersInCountdown ();
            startTick = Environment.TickCount;

            countdownTimer = Timer.SetTimer ( StartRound, countdownTime + 400 );
        }

        private void StartRound ( ) {
            status = LobbyStatus.ROUND;
            NAPI.Util.ConsoleOutput ( status.ToString () );
            startTick = Environment.TickCount;
            roundEndTimer = Timer.SetTimer ( EndRound, roundTime );
            alivePlayers = new List<List<NetHandle>> ();
            List<uint> amountinteams = new List<uint> ();
            for ( int i = 0; i < Players.Count; i++ ) {
                uint amountinteam = (uint) Players[i].Count;
                if ( i != 0 )
                    amountinteams.Add ( amountinteam );
                alivePlayers.Add ( new List<NetHandle> () );
                for ( int j = 0; j < amountinteam; j++ ) {
                    NAPI.Util.ConsoleOutput ( "StartRound " + i + ":" + j + " " + NAPI.Player.GetPlayerFromHandle ( Players[i][j] ).Name );
                    StartRoundForPlayer ( NAPI.Player.GetPlayerFromHandle ( Players[i][j] ), (uint) i );
                }
            }

            PlayerAmountInFightSync ( amountinteams );

            if ( currentMap.SyncData.Type == MapType.BOMB )
                StartRoundBomb ();
        }

        private void StartRoundForPlayer ( Client player, uint teamID ) {
            Character character = player.GetChar ();
            NAPI.Util.ConsoleOutput ( "StartRoundForPlayer " + player.Name );
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientRoundStart", teamID == 0 ? 1 : 0 );
            character.Team = (ushort) teamID;
            if ( teamID != 0 ) {
                character.Lifes = (ushort) Lifes;
                alivePlayers[(int) teamID].Add ( player );
                player.Freeze ( false );
            }
        }

        private void EndRound ( ) {
            status = LobbyStatus.ROUNDEND;
            NAPI.Util.ConsoleOutput ( status.ToString () );
            roundStartTimer?.Kill ();
            DeleteMapBlips ();
            if ( currentMap != null && currentMap.SyncData.Type == MapType.BOMB )
                StopRoundBombAtRoundEnd ();
            if ( IsSomeoneInLobby () ) {
                roundStartTimer = Timer.SetTimer ( StartMapChoose, RoundEndTime / 2 );
                SendAllPlayerEvent ( "onClientRoundEnd" );
            } else if ( DeleteWhenEmpty ) {
                Remove ();
            }
        }

        public void EndRoundEarlier ( ) {
            roundEndTimer?.Kill ();
            countdownTimer?.Kill ();
            EndRound ();
        }

        private void RespawnPlayerInRound ( Client player ) {
            SetPlayerReadyForRound ( player, player.GetChar ().Team );
            player.Freeze ( false );
        }
    }
}