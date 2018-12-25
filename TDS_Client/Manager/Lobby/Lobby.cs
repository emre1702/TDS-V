using System;
using System.Collections.Generic;
using System.Text;
using TDS_Common.Dto;

namespace TDS_Client.Manager.Lobby
{
    static class Lobby
    {
        private static bool inMainMenu;

        public static void Joined(SyncedLobbySettingsDto settings)
        {
            if (inMainMenu)
                LeftMainMenu();
            else 
                Left();
            if (settings.Id == 0)
            {
                JoinedMainmenu();
                return;
            }
            inMainMenu = false;

            //SetMapInfo
            
        }

        private static void Left()
        {
            RoundInfo.Stop();
            /*
             * toggleFightMode( false );
		removeBombThings();
		removeRoundThings( true );
        stopCountdownCamera();
        closeMapVotingMenu();  
        clearMapVotingsInBrowser();
        removeRoundInfo();
        stopMapCreator();
        hideRoundEndReason();*/
        }

        private static void JoinedMainmenu()
        {
            inMainMenu = true;
            RAGE.Game.Cam.DoScreenFadeIn(100);
            //startLobbyChoiceBrowser();
        }

        private static void LeftMainMenu()
        {
            //destroyLobbyChoiceBrowser();
        }

        /*mp.events.add( "onClientPlayerJoinMapCreatorLobby", () => {
    startMapCreator();
} );*/
    }
}
