using RAGE.Game;
using TDS_Client.Manager.Lobby;

namespace TDS_Client.Manager.Utility
{
    static class AntiCheatManager
    {
        public static void OnTick()
        {
            Player.SetPlayerTargetingMode(0);
            Player.SetPlayerLockon(false);

            if (Round.InFight)
            {
                if (Player.GetPlayerInvincible())
                {
                    //Todo: Log to server
                    Player.SetPlayerInvincible(false);
                }


                
            }

        }
    }
}
