using GTANetworkAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.server.enums;
using TDS.server.extend;
using TDS.server.instance.player;
using TDS.server.instance.utility;
using TDS.server.manager.utility;

namespace TDS.server.instance.lobby {

    partial class Arena {

        private Timer roundStartTimer,
                        countdownTimer,
                        roundEndTimer;
        private uint countdownTime = 5 * 1000;
        private uint roundTime = 4 * 60 * 1000;
        public uint RoundEndTime = 8 * 1000;
        private uint mapShowTime = 4 * 1000;
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
                SendAllPlayerEvent ( "onClientMapChange", -1, currentMap.SyncData.Name, JsonConvert.SerializeObject ( currentMap.MapLimits ), currentMap.MapCenter.X, currentMap.MapCenter.Y, currentMap.MapCenter.Z );

                roundStartTimer = Timer.SetTimer ( StartRoundCountdown, mapShowTime );
            } catch ( Exception ex ) {
                NAPI.Util.ConsoleOutput ( ex.ToString() );
            }
        }

        private void StartRoundCountdown ( ) {
            status = LobbyStatus.COUNTDOWN;
            NAPI.Util.ConsoleOutput ( status.ToString () );
            spectatingMe.Clear ();
            SetAllPlayersInCountdown ();
            startTick = Environment.TickCount;

            countdownTimer = Timer.SetTimer ( StartRound, countdownTime + 400 );
        }

        private void StartRound ( ) {
            status = LobbyStatus.ROUND;
            NAPI.Util.ConsoleOutput ( status.ToString () );
            startTick = Environment.TickCount;
            roundEndTimer = Timer.SetTimer ( EndRoundTimesup, roundTime );
            alivePlayers = new List<List<Character>> ();
            List<uint> amountinteams = new List<uint> ();
            for ( int i = 0; i < Players.Count; i++ ) {
                uint amountinteam = (uint) Players[i].Count;
                if ( i != 0 )
                    amountinteams.Add ( amountinteam );
                alivePlayers.Add ( new List<Character> () );
                for ( int j = 0; j < amountinteam; j++ ) {
                    StartRoundForPlayer ( Players[i][j], i );
                }
            }

            PlayerAmountInFightSync ( amountinteams );

            if ( currentMap.SyncData.Type == MapType.BOMB )
                StartRoundBomb ();
        }

        private void EndRound ( Dictionary<Language, string> reasonlangs ) {
            status = LobbyStatus.ROUNDEND;
            NAPI.Util.ConsoleOutput ( status.ToString () );
            roundStartTimer?.Kill ();
            DeleteMapBlips ();
            if ( currentMap != null && currentMap.SyncData.Type == MapType.BOMB )
                StopRoundBombAtRoundEnd ();
            if ( IsSomeoneInLobby () ) {
                roundStartTimer = Timer.SetTimer ( StartMapChoose, RoundEndTime );
                FuncIterateAllPlayers ( ( character, teamID ) => {
                    NAPI.ClientEvent.TriggerClientEvent ( character.Player, "onClientRoundEnd", reasonlangs[character.Language] );
                } );                     
            } else if ( DeleteWhenEmpty ) {
                Remove ();
            }
        }

        private void EndRoundTimesup ( ) {
            EndRound ( GetRoundEndReasonLang ( RoundEndReason.TIME, null ) );
        }

        public void EndRoundEarlier ( RoundEndReason reason, object arg ) {
            roundEndTimer?.Kill ();
            countdownTimer?.Kill ();
            EndRound ( GetRoundEndReasonLang ( reason, arg ) );
        }

        private Dictionary<Language, string> GetRoundEndReasonLang ( RoundEndReason reasonenum, object arg ) {
            Dictionary<Language, string> reasons;
            switch ( reasonenum ) {
                case RoundEndReason.DEATH:
                    if ( (int)arg == 0 )
                        reasons = ServerLanguage.GetLangDictionary ( "round_end_death_all" );
                    else 
                        reasons = ServerLanguage.GetLangDictionary ( "round_end_death", GetTeamName ( (int) arg ) );
                    break;
                case RoundEndReason.TIME:
                    reasons = ServerLanguage.GetLangDictionary ( "round_end_time" );
                    break;
                case RoundEndReason.BOMB:
                    int teamID = (int) arg;
                    if ( teamID == terroristTeamID )
                        reasons = ServerLanguage.GetLangDictionary ( "round_end_bomb_exploded", GetTeamName ( teamID ) );
                    else
                        reasons = ServerLanguage.GetLangDictionary ( "round_end_bomb_defused", GetTeamName ( teamID ) );
                    break;
                case RoundEndReason.COMMAND:
                    reasons = ServerLanguage.GetLangDictionary ( "round_end_command", (string) arg );
                    break;
                case RoundEndReason.NEWPLAYER:
                    reasons = ServerLanguage.GetLangDictionary ( "round_end_command", (string) arg );
                    break;
                default:
                    reasons = new Dictionary<Language, string> ();   // Only to not get an error! Won't be used & can't be used!
                    break;
            }
            return reasons;
        } 

        private void StartRoundForPlayer ( Character character, int teamID ) {
            NAPI.ClientEvent.TriggerClientEvent ( character.Player, "onClientRoundStart", teamID == 0 ? 1 : 0 );
            if ( teamID != 0 ) {
                character.Lifes = Lifes;
                alivePlayers[teamID].Add ( character );
                character.Player.Freeze ( false );
            }
        }

        private void RespawnPlayerInRound ( Character character ) {
            SetPlayerReadyForRound ( character );
            character.Player.Freeze ( false );
        }
    }
}