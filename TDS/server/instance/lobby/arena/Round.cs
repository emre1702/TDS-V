﻿using GTANetworkAPI;
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


        public void StartRoundGame ( ) {
            StartMapChoose ();
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
                SendAllPlayerEvent ( "onClientMapChange", -1, currentMap.SyncData.Name, JsonConvert.SerializeObject ( maplimits ), currentMap.MapCenter.X, currentMap.MapCenter.Y, currentMap.MapCenter.Z );

                roundStartTimer = Timer.SetTimer ( StartRoundCountdown, RoundEndTime / 2 );
            } catch ( Exception ex ) {
                NAPI.Util.ConsoleOutput ( ex.ToString() );
            }
        }

        private void StartRoundCountdown ( ) {
            status = LobbyStatus.COUNTDOWN;
            NAPI.Util.ConsoleOutput ( status.ToString () );
            spectatingMe = new Dictionary<Client, List<Client>> ();
            SetAllPlayersInCountdown ();
            startTick = Environment.TickCount;

            countdownTimer = Timer.SetTimer ( StartRound, countdownTime + 400 );
        }

        private void StartRound ( ) {
            status = LobbyStatus.ROUND;
            NAPI.Util.ConsoleOutput ( status.ToString () );
            startTick = Environment.TickCount;
            roundEndTimer = Timer.SetTimer ( () => EndRound ( RoundEndReason.TIME ), roundTime );
            alivePlayers = new List<List<Client>> ();
            List<uint> amountinteams = new List<uint> ();
            for ( int i = 0; i < Players.Count; i++ ) {
                uint amountinteam = (uint) Players[i].Count;
                if ( i != 0 )
                    amountinteams.Add ( amountinteam );
                alivePlayers.Add ( new List<Client> () );
                for ( int j = 0; j < amountinteam; j++ ) {
                    StartRoundForPlayer ( Players[i][j], (uint) i );
                }
            }

            PlayerAmountInFightSync ( amountinteams );

            if ( currentMap.SyncData.Type == MapType.BOMB )
                StartRoundBomb ();
        }

        private void EndRound ( RoundEndReason reason, params object[] args ) {
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

        public void EndRoundEarlier ( RoundEndReason reason, params object[] args ) {
            roundEndTimer?.Kill ();
            countdownTimer?.Kill ();
            EndRound ( reason, args );
        }

        private void StartRoundForPlayer ( Client player, uint teamID ) {
            Character character = player.GetChar ();
            NAPI.ClientEvent.TriggerClientEvent ( player, "onClientRoundStart", teamID == 0 ? 1 : 0 );
            if ( teamID != 0 ) {
                character.Lifes = (ushort) Lifes;
                alivePlayers[(int) teamID].Add ( player );
                player.Freeze ( false );
            }
        }

        private void RespawnPlayerInRound ( Client player ) {
            SetPlayerReadyForRound ( player, player.GetChar ().Team );
            player.Freeze ( false );
        }

        private static List<Tuple<float, float>> GetJsonSerializableList ( List<Vector3> list ) {
            List<Tuple<float, float>> newlist = new List<Tuple<float, float>> ();
            foreach ( Vector3 vector in list ) {
                newlist.Add ( new Tuple<float, float> ( vector.X, vector.Y ) );
            }
            return newlist;
        }
    }
}