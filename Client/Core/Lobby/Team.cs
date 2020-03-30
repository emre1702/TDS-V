using RAGE.Game;
using System.Collections.Generic;
using TDS_Client.Enum;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;
using TDS_Shared.Dto;
using TDS_Shared.Enum;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Lobby
{
    internal static class Team
    {
        public static List<SyncedTeamDataDto> LobbyTeams 
        { 
            get => _LobbyTeams;
            set
            {
                _LobbyTeams = value;
                if (_LobbyTeams != null)
                {
                    if (_LobbyTeams.Count == 1)
                    {
                        Player.LocalPlayer.SetCanAttackFriendly(true, true);
                    }
                    else
                    {
                        Player.LocalPlayer.SetCanAttackFriendly(false, false);
                    }
                    
                }
            }
        }
        private static HashSet<Player> _sameTeamPlayers { get; set; } = new HashSet<Player>();
        public static string CurrentTeamName { get; set; } = "Login/Register";
        public static int AmountPlayersSameTeam => _sameTeamPlayers.Count;

        private static bool _activated;
        private static List<SyncedTeamDataDto> _LobbyTeams;

        public static void Init()
        {
            BindManager.Add(EKey.NumPad0, ToggleOrderMode);
            int i = 0;
            foreach (var orderobj in System.Enum.GetValues(typeof(TeamOrder)))
            {
                BindManager.Add(EKey.NumPad1 + i, GiveOrder);
                ++i;
            }
        }

        public static void AddSameTeam(Player player)
        {
            //Todo: If the server crashes after a teammate dies, it could be because of the blip.
            // Then try removing the blip on death
            _sameTeamPlayers.Add(player);
            var prevBlipHandle = player.GetBlipFrom();
            RAGE.Chat.Output("Prev blip handle: " + prevBlipHandle);
            if (prevBlipHandle <= 0)
            {
                var blip = player.AddBlipFor();
                RAGE.Chat.Output("New blip handle: " + blip);
                Ui.SetBlipAsFriendly(blip, true);
            }
        }

        public static void RemoveSameTeam(Player player)
        {
            _sameTeamPlayers.Remove(player);
            var prevBlipHandle = player.GetBlipFrom();
            RAGE.Chat.Output("Prev blip handle @RemoveSameTeam: " + prevBlipHandle);
            if (prevBlipHandle > 0 && Ui.DoesBlipExist(prevBlipHandle))
               Ui.RemoveBlip(ref prevBlipHandle);
        }

        public static void ClearSameTeam()
        {
            foreach (var player in _sameTeamPlayers)
            {
                var blip = player.GetBlipFrom();
                RAGE.Chat.Output("Prev blip handle @ClearSameTeam: " + blip);
                if (blip > 0 && Ui.DoesBlipExist(blip))
                    Ui.RemoveBlip(ref blip);
            }
            _sameTeamPlayers.Clear();
        }

        public static bool IsInSameTeam(Player player)
        {
            return _sameTeamPlayers.Contains(player);
        }

        public static void ToggleOrderMode(EKey _)
        { 
            if (!_activated && Browser.Angular.Shared.InInput)
                return;
            _activated = !_activated;
            Browser.Angular.Main.ToggleTeamOrderModus(_activated);
        }

        private static void GiveOrder(EKey key)
        {
            if (!_activated)
                return;
            if (!Lobby.InFightLobby)
                return;
            if (Browser.Angular.Shared.InInput)
                return;

            TeamOrder order = GetTeamOrderByKey(key);
            EventsSender.Send(ToServerEvent.SendTeamOrder, (int)order);
            ToggleOrderMode(EKey.NoName);
        }

        private static TeamOrder GetTeamOrderByKey(EKey key)
        {
            return (TeamOrder)(key - EKey.NumPad1);
        }
    }
}
