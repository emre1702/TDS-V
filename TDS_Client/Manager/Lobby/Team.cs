using Newtonsoft.Json;
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
    internal static class Team
    {
        public static SyncedTeamDataDto[] CurrentLobbyTeams;
        private static HashSet<Player> _sameTeamPlayers { get; set; } = new HashSet<Player>();
        public static string CurrentTeamName { get; set; } = "Login/Register";

        private static bool _activated;

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

        public static void AddSameTeam(Player player)
        {
            _sameTeamPlayers.Add(player);
        }

        public static void RemoveSameTeam(Player player)
        {
            _sameTeamPlayers.Remove(player);
        }

        public static void ClearSameTeam()
        {
            _sameTeamPlayers.Clear();
        }

        public static bool IsInSameTeam(Player player)
        {
            return _sameTeamPlayers.Contains(player);
        }

        public static void ToggleOrderMode(ConsoleKey _)
        {
            _activated = !_activated;
            Angular.ToggleTeamOrderModus(_activated);
        }

        private static void GiveOrder(ConsoleKey key)
        {
            if (!_activated)
                return;
            if (!Lobby.InFightLobby)
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