using Newtonsoft.Json;
using RAGE.Game;
using System;
using System.Collections.Generic;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Lobby
{
    static class Team
    {
        public static SyncedTeamDataDto[] CurrentLobbyTeams;
        public static List<Player> SameTeamPlayers = new List<Player>();
        public static string CurrentTeamName { get; set; } = "Login/Register";

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

        public static void LoadOrderNames()
        {
            MainBrowser.LoadOrderNamesInBrowser(JsonConvert.SerializeObject(Settings.Language.ORDER));
        }


        public static void ToggleOrderMode(ConsoleKey _)
        {
            activated = !activated;
            MainBrowser.ToggleCanVoteForMapWithNumpadInBrowser(!activated);
        }

        private static void GiveOrder(ConsoleKey key)
        {
            if (!activated)
                return;
            ETeamOrder order = GetTeamOrderByKey(key);
            EventsSender.Send(DToServerEvent.SendTeamOrder, (int)order);
            ToggleOrderMode(ConsoleKey.NoName);
        }

        private static ETeamOrder GetTeamOrderByKey(ConsoleKey key)
        {
            return (ETeamOrder)(key - ConsoleKey.NumPad1);
        }
    }
}
