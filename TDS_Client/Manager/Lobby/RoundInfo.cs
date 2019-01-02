using RAGE.Game;
using System;
using System.Drawing;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Dto;
using TDS_Common.Manager.Utility;

namespace TDS_Client.Manager.Lobby
{
    static class RoundInfo
    {
        private static ulong startedTick;

        private static readonly DxTextRectangle timeDisplay = new DxTextRectangle("00:00", 0.5f, 0.025f, 0.06f, 0.05f, 
                            Color.White, Color.FromArgb(180, 20, 20, 20), textScale: 0.5f, alignment: Alignment.Center, activated: false);
        private static DxTextRectangle[] teamDisplays;

        public static bool RefreshOnTick { get; set; }

        public static void Start(ulong alreadywastedms)
        {
            startedTick = TimerManager.ElapsedTicks - alreadywastedms;

            timeDisplay.Activated = true;
            RefreshOnTick = true;

            var teams = Team.CurrentLobbyTeams;
            int showamountleft = (int) Math.Ceiling((teams.Length - 1) / 2d);
            int showamountright = teams.Length - showamountleft - 1;
            for (int i = 0; i < showamountleft; ++i)
            {
                float x = 0.5f - 0.06f*0.5f - 0.13f*i - 0.13f * 0.5f;
                teamDisplays[i] = new DxTextRectangle(teams[i+1].Name, x, 0.03f, 0.13f, 0.06f, Color.White, teams[i+1].Color, 0.41f, alignment: Alignment.Center);
            }
            for (int j = 0; j < showamountright; ++j)
            {
                float x = 0.5f + 0.06f*0.5f + 0.13f * j + 0.13f * 0.5f;
                int i = j + showamountleft;
                teamDisplays[i] = new DxTextRectangle(teams[i+1].Name, x, 0.03f, 0.13f, 0.06f, Color.White, teams[i+1].Color, 0.41f, alignment: Alignment.Center);
            }
        }

        public static void Stop()
        {
            RefreshOnTick = false;
            timeDisplay.Activated = false;
            foreach (var display in teamDisplays)
            {
                display.Remove();
            }

        }

        private static void Remove()
        {
            RefreshOnTick = false;
            timeDisplay.Activated = false;
            foreach (var display in teamDisplays)
            {
                display.Remove();
            }
            teamDisplays = null;
        }

        public static void RefreshTime()
        {
            ulong timems = Settings.RoundTime*1000 - (TimerManager.ElapsedTicks - startedTick);
            timeDisplay.SetText(TimeSpan.FromMilliseconds(timems).ToString(@"mm\:ss"));
        }

        public static void RefreshTeamText(int index)
        {
            SyncedTeamDataDto team = Team.CurrentLobbyTeams[index];
            teamDisplays[index].SetText(team.Name + "\n" + team.AmountPlayers.AmountAlive + "/" + team.AmountPlayers.Amount);
        }

        public static void SetRoundTimeLeft(ulong lefttimems)
        {
            startedTick = TimerManager.ElapsedTicks - (Settings.RoundTime * 1000 - lefttimems);
        }

        public static void OnePlayerDied(int teamindex, string killinfostr)
        {
            --Team.CurrentLobbyTeams[teamindex].AmountPlayers.AmountAlive;
            RefreshTeamText(teamindex);
            MainBrowser.AddKillMessage(killinfostr);
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
