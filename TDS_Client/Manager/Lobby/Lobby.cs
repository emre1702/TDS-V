using TDS_Client.Manager.Browser;
using TDS_Common.Dto;

namespace TDS_Client.Manager.Lobby
{
    internal static class Lobby
    {
        private static bool _inMainMenu;

        public static void Joined(SyncedLobbySettingsDto settings)
        {
            if (_inMainMenu)
                LeftMainMenu();
            else
                Left();
            if (settings.Id == 0)
            {
                JoinedMainmenu();
                return;
            }
            _inMainMenu = false;
            //SetMapInfo
        }

        private static void Left()
        {
            RoundInfo.Stop();
            Round.InFight = false;
            Bomb.Reset();
            Round.Reset(true);
            LobbyCam.StopCountdown();
            MapManager.CloseMenu();
            MainBrowser.ClearMapVotingsInBrowser();
            RoundInfo.Stop();
            /*stopMapCreator();
            hideRoundEndReason();*/
        }

        private static void JoinedMainmenu()
        {
            _inMainMenu = true;
            RAGE.Game.Cam.DoScreenFadeIn(100);
            Choice.Start();
        }

        private static void LeftMainMenu()
        {
            Choice.Stop();
        }

        /*mp.events.add( "onClientPlayerJoinMapCreatorLobby", () => {
    startMapCreator();
} );*/
    }
}