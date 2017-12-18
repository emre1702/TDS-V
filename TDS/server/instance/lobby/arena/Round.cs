using GTANetworkAPI;
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

        private List<List<Client>> alivePlayers = new List<List<Client>> {
            new List<Client> ()
        };

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

        public async void StartMapChoose ( ) {
            try {
                status = LobbyStatus.MAPCHOOSE;
                NAPI.Util.ConsoleOutput ( status.ToString () );
                if ( IsOfficial )
                    RewardAllPlayer ();
                DmgSys.EmptyDamagesysData ();

                await Task.Run ( ( ) => {
                    NAPI.Task.Run ( async ( ) => {
                        if ( currentMap != null && currentMap.Type == MapType.BOMB )
                            StopRoundBomb ();
                        currentMap = await GetNextMap ().ConfigureAwait ( false );
                        if ( currentMap.Type == MapType.BOMB )
                            BombMapChose ();
                        CreateTeamSpawnBlips ();
                        CreateMapLimitBlips ();
                        if ( mixTeamsAfterRound )
                            MixTeams ();
                        SendAllPlayerEvent ( "onClientMapChange", -1, currentMap.MapLimits, currentMap.MapCenter );
                    } );
                } );

                roundStartTimer = Timer.SetTimer ( StartRoundCountdown, RoundEndTime / 2 );
            } catch ( Exception ex ) {
                NAPI.Util.ConsoleOutput ( "Error in StartMapChoose:" + ex.Message );
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
            roundEndTimer = Timer.SetTimer ( EndRound, roundTime );
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

            if ( currentMap.Type == MapType.BOMB )
                StartRoundBomb ();
        }

        private void StartRoundForPlayer ( Client player, uint teamID ) {
            Character character = player.GetChar ();
            player.TriggerEvent ( "onClientRoundStart", teamID == 0 );
            character.Team = teamID;
            if ( teamID != 0 ) {
                character.Lifes = Lifes;
                alivePlayers[(int) teamID].Add ( player );
                player.Freeze ( false );
            }
        }

        private void EndRound ( ) {
            status = LobbyStatus.ROUNDEND;
            NAPI.Util.ConsoleOutput ( status.ToString () );
            roundStartTimer.Kill ();
            DeleteMapBlips ();
            if ( currentMap.Type == MapType.BOMB )
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