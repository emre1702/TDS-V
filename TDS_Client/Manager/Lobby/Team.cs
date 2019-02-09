using RAGE;
using RAGE.Game;
using System;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Lobby
{
    static class Team
    {
        public static SyncedTeamDataDto[] CurrentLobbyTeams;
        public static string CurrentTeamName = "Login/Register";

        private static bool activated;

        public static void Init()
        {
            BindManager.Add(ConsoleKey.NumPad0, ToggleOrderMode);
            int i = 0;
            foreach (var orderobj in System.Enum.GetValues(typeof(ETeamOrder)))
            {
                BindManager.Add(ConsoleKey.NumPad1 + i, GiveOrder);
                ++i;
            }
        }
        
        public static void ToggleOrderMode(ConsoleKey _)
        {
            activated = !activated;
            MainBrowser.ToggleCanVoteForMapWithNumpadInBrowser(!activated);
        }

        private static void GiveOrder(ConsoleKey key)
        {
            ETeamOrder order = GetTeamOrderByKey(key);
            Events.CallRemote(DToServerEvent.SendTeamOrder, (int)order);
        }

        private static ETeamOrder GetTeamOrderByKey(ConsoleKey key)
        {
            return (ETeamOrder)(ConsoleKey.NumPad1 - key);
        }
    }
}
