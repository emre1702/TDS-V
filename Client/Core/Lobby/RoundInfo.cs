using RAGE.NUI;
using System;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Shared.Dto;

namespace TDS_Client.Manager.Lobby
{
    internal static class RoundInfo
    {
        private static ulong _startedTick;
        /*private static int _currentAssists;
        private static int _currentDamage;
        private static int _currentKills;*/

        private static DxTextRectangle _timeDisplay;
        private static DxTextRectangle[] _teamDisplays;

        /*public static int CurrentAssists
        {
            get => _currentAssists;
            set
            {
                _currentAssists = value;
                ShowDeathmatchInfo();
            }
        }

        public static int CurrentDamage
        {
            get => _currentDamage;
            set
            {
                _currentDamage = value;
                ShowDeathmatchInfo();
            }
        }

        public static int CurrentKills
        {
            get => _currentKills;
            set
            {
                _currentKills = value;
                ShowDeathmatchInfo();
            }
        }*/

        public static bool RefreshOnTick 
        { 
            get => _refreshOnTick;
            set
            {
                if (_refreshOnTick == value)
                    return;
                _refreshOnTick = value;
                if (value)
                    TickManager.Add(RefreshTime);
                else 
                    TickManager.Remove(RefreshTime);
            }
        }

        private static bool _refreshOnTick;

        public static void Start(ulong elapsedMsSinceRoundStart)
        {
            _startedTick = TimerManager.ElapsedTicks - elapsedMsSinceRoundStart;

            if (_teamDisplays != null)
                Stop();

            CreateTimeDisplay();
            CreateTeamsDisplays();
        }

        private static void CreateTimeDisplay()
        {
            if (_timeDisplay == null)
                _timeDisplay = new DxTextRectangle("00:00", 0.5f, 0f, 0.06f, 0.05f,
                            Color.White, Color.FromArgb(180, 20, 20, 20), textScale: 0.5f, alignmentX: UIResText.Alignment.Centered, ToServerEvent: ToServerEvent.Top);
            _timeDisplay.Activated = true;
            RefreshOnTick = true;
        }

        private static void CreateTeamsDisplays()
        {
            var teams = Team.LobbyTeams;
            int showamountleft = (int)Math.Ceiling((teams.Count - 1) / 2d);
            int showamountright = teams.Count - showamountleft - 1;
            _teamDisplays = new DxTextRectangle[teams.Count - 1];
            for (int i = 0; i < showamountleft; ++i)
            {
                float x = 0.5f - 0.06f * 0.5f - 0.13f * i - 0.13f * 0.5f;
                var team = teams[i + 1];
                _teamDisplays[i] = new DxTextRectangle(team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount, x, 0, 0.13f, 0.06f, Color.White,
                    Color.FromArgb(187, team.Color.R, team.Color.G, team.Color.B), 0.41f, alignmentX: UIResText.Alignment.Centered, ToServerEvent: ToServerEvent.Top, amountLines: 2);
            }
            for (int j = 0; j < showamountright; ++j)
            {
                float x = 0.5f + 0.06f * 0.5f + 0.13f * j + 0.13f * 0.5f;
                int i = j + showamountleft;
                var team = teams[i + 1];
                _teamDisplays[i] = new DxTextRectangle(team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount, x, 0, 0.13f, 0.06f, Color.White,
                    Color.FromArgb(187, team.Color.R, team.Color.G, team.Color.B), 0.41f, alignmentX: UIResText.Alignment.Centered, ToServerEvent: ToServerEvent.Top, amountLines: 2);
            }
        }

        public static void Stop()
        {
            RefreshOnTick = false;
            if (_timeDisplay != null)
                _timeDisplay.Activated = false;
            if (_teamDisplays != null)
            {
                foreach (var display in _teamDisplays)
                {
                    display.Remove();
                }
                _teamDisplays = null;
            }
        }

        public static void StopDeathmatchInfo()
        {
            // stop damage, assists and kills display here
        }

        public static void RefreshTime()
        {
            var elapsedMs = (int)(TimerManager.ElapsedTicks - _startedTick);
            double timems = 0;
            if (Settings.RoundTime * 1000 > elapsedMs)
                timems = (ulong)Settings.RoundTime * 1000 - (TimerManager.ElapsedTicks - _startedTick);
            _timeDisplay?.SetText(TimeSpan.FromMilliseconds(timems).ToString(@"mm\:ss"));
        }

        public static void RefreshAllTeamTexts()
        {
            foreach (var team in Team.LobbyTeams)
            {
                if (team.IsSpectator)
                    continue;
                RefreshTeamText(team.Index);
            }
        }

        public static void RefreshTeamText(int index)
        {
            if (_teamDisplays == null)
                return;
            SyncedTeamDataDto team = Team.LobbyTeams[index];
            _teamDisplays[index - 1].SetText(team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount);
        }

        public static void SetRoundTimeLeft(int lefttimems)
        {
            _startedTick = TimerManager.ElapsedTicks - ((ulong)Settings.RoundTime * 1000 - (ulong)lefttimems);
        }

        public static void OnePlayerDied(int teamindex)
        {
            --Team.LobbyTeams[teamindex].AmountPlayers.AmountAlive;
            RefreshTeamText(teamindex);
        }

        private static void ShowDeathmatchInfo()
        {
            // show info of kills, damage, assists if not already showing
        }

        /*
       mp.events.add( "onClientPlayerAmountInFightSync", ( amountinteam, isroundstarted, amountaliveinteam ) => {
   log( "onClientPlayerAmountInFightSync" );
   roundinfo.amountinteams = [];
   roundinfo.aliveinteams = [];
   amountinteam = JSON.parse( amountinteam );
   if ( isroundstarted == 1)
       amountaliveinteam = JSON.parse( amountaliveinteam );
   for ( let i = 0; i < amountinteam.length; i++ ) {
       roundinfo.amountinteams[i] = Number.parseInt( amountinteam[i] );
       if ( isroundstarted == 0 )
           roundinfo.aliveinteams[i] = Number.parseInt ( amountinteam[i] );
       else
           roundinfo.aliveinteams[i] = Number.parseInt ( amountaliveinteam[i] );
   }
   refreshRoundInfoTeamData();
} );
*/
    }
}
