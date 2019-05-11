using RAGE.NUI;
using System;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Dto;

namespace TDS_Client.Manager.Lobby
{
    internal static class RoundInfo
    {
        private static ulong startedTick;
        private static int _currentAssists;
        private static int _currentDamage;
        private static int _currentKills;

        private static DxTextRectangle timeDisplay;
        private static DxTextRectangle[] teamDisplays;

        public static int CurrentAssists
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
        }

        public static bool RefreshOnTick { get; set; }

        public static void Start(ulong alreadywastedms)
        {
            startedTick = TimerManager.ElapsedTicks - alreadywastedms;

            if (teamDisplays != null)
                Stop();

            CreateTimeDisplay();
            CreateTeamsDisplays();
        }

        private static void CreateTimeDisplay()
        {
            if (timeDisplay == null)
                timeDisplay = new DxTextRectangle("00:00", 0.5f, 0f, 0.06f, 0.05f,
                            Color.White, Color.FromArgb(180, 20, 20, 20), textScale: 0.5f, alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Top);
            timeDisplay.Activated = true;
            RefreshOnTick = true;
        }

        private static void CreateTeamsDisplays()
        {
            var teams = Team.CurrentLobbyTeams;
            int showamountleft = (int)Math.Ceiling((teams.Length - 1) / 2d);
            int showamountright = teams.Length - showamountleft - 1;
            teamDisplays = new DxTextRectangle[teams.Length - 1];
            for (int i = 0; i < showamountleft; ++i)
            {
                float x = 0.5f - 0.06f * 0.5f - 0.13f * i - 0.13f * 0.5f;
                var team = teams[i + 1];
                teamDisplays[i] = new DxTextRectangle(team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount, x, 0, 0.13f, 0.06f, Color.White, team.Color, 0.41f, alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Top, amountLines: 2);
            }
            for (int j = 0; j < showamountright; ++j)
            {
                float x = 0.5f + 0.06f * 0.5f + 0.13f * j + 0.13f * 0.5f;
                int i = j + showamountleft;
                var team = teams[i + 1];
                teamDisplays[i] = new DxTextRectangle(team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount, x, 0, 0.13f, 0.06f, Color.White, team.Color, 0.41f, alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Top, amountLines: 2);
            }
        }

        public static void Stop()
        {
            RefreshOnTick = false;
            if (timeDisplay != null)
                timeDisplay.Activated = false;
            if (teamDisplays != null)
            {
                foreach (var display in teamDisplays)
                {
                    display.Remove();
                }
                teamDisplays = null;
            }
        }

        public static void StopDeathmatchInfo()
        {
            // stop damage, assists and kills display here
        }

        public static void RefreshTime()
        {
            double timems = Math.Max(0, (ulong)Settings.RoundTime * 1000 - (TimerManager.ElapsedTicks - startedTick));
            timeDisplay?.SetText(TimeSpan.FromMilliseconds(timems).ToString(@"mm\:ss"));
        }

        public static void RefreshAllTeamTexts()
        {
            foreach (var team in Team.CurrentLobbyTeams)
            {
                if (team.Index == 0)
                    continue;
                RefreshTeamText(team.Index);
            }
        }

        public static void RefreshTeamText(int index)
        {
            if (teamDisplays == null)
                return;
            SyncedTeamDataDto team = Team.CurrentLobbyTeams[index];
            teamDisplays[index - 1].SetText(team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount);
        }

        public static void SetRoundTimeLeft(int lefttimems)
        {
            startedTick = TimerManager.ElapsedTicks - ((ulong)Settings.RoundTime * 1000 - (ulong)lefttimems);
        }

        public static void OnePlayerDied(int teamindex, string killinfostr)
        {
            --Team.CurrentLobbyTeams[teamindex].AmountPlayers.AmountAlive;
            RefreshTeamText(teamindex);
            MainBrowser.AddKillMessage(killinfostr);
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