using GTANetworkAPI;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TDS_Common.Default;
using TDS_Server.Dto.TeamChoiceMenu;
using TDS_Server.Instance.Lobby;
using TDS_Server.Instance.Player;
using TDS_Server.Instance.Utility;

namespace TDS_Server.Manager.Sync
{
    class TeamChoiceMenuSync
    {
        private static readonly List<TDSPlayer> _playersInTeamChoiceLobby = new List<TDSPlayer>();

        public static void AddPlayer(TDSPlayer player, Lobby lobby)
        {
            _playersInTeamChoiceLobby.Add(player);

            var teams = lobby.Teams.Select(t => 
                new TeamChoiceMenuTeamData(t.Entity.Name, t.Entity.ColorR, t.Entity.ColorG, t.Entity.ColorB, t.Players.Where(p => p != player).Select(p => p.Client.Name)));

            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SyncTeamChoiceMenuData, JsonConvert.SerializeObject(teams));
        }

        public static void RemovePlayer(TDSPlayer player)
        {
            if (_playersInTeamChoiceLobby.Contains(player))
            {
                _playersInTeamChoiceLobby.Remove(player);
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.ToggleTeamChoiceMenu, false);
            }
        }

        public static void PlayerChangedTeam(TDSPlayer player, Team? newTeam, Team? oldTeam)
        {
            if (player.CurrentLobby is null)
                return;

            _playersInTeamChoiceLobby.RemoveAll(p => !p.LoggedIn || !p.Client.Exists);
            var playersToSyncTo = _playersInTeamChoiceLobby.Where(p => p.CurrentLobby != null && p.CurrentLobby == player.CurrentLobby);
            foreach (var target in playersToSyncTo)
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SetPlayerNameToTeamChoiceData, player.Client.Name, newTeam?.Entity.Name, oldTeam?.Entity.Name);
            }
        }
    }
}
