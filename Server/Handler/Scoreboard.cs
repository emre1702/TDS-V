using AltV.Net;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Interfaces.Handlers;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Server.Handler
{
    public class ScoreboardHandler
    {
        #region Private Fields

        private readonly LobbiesHandler _lobbiesHandler;
        private readonly Serializer _serializer;

        #endregion Private Fields

        #region Public Constructors

        public ScoreboardHandler(Serializer serializer, LobbiesHandler lobbiesHandler)
        {
            _serializer = serializer;
            _lobbiesHandler = lobbiesHandler;

            Alt.OnClient<ITDSPlayer>(ToServerEvent.RequestPlayersForScoreboard, OnRequestPlayersForScoreboard);
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnRequestPlayersForScoreboard(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;

            if (player.Lobby is null || GetShowAllLobbies(player.Lobby.Type))
            {
                var entries = GetDataForMainmenu();
                player.SendEvent(ToClientEvent.SyncScoreboardData, _serializer.ToClient(entries));
            }
            else
            {
                var entries = GetDataForLobby(player.Lobby.Id);
                if (entries is null)
                    return;
                var lobbydata = GetDataForMainmenu().Where(d => d.Id != player.Lobby?.Id);
                player.SendEvent(ToClientEvent.SyncScoreboardData, _serializer.ToClient(entries), _serializer.ToClient(lobbydata));
            }
        }

        #endregion Public Methods

        #region Private Methods

        private List<SyncedScoreboardLobbyDataDto>? GetDataForLobby(int lobbyId)
        {
            ILobby? lobby = _lobbiesHandler.Lobbies.Where(l => l.Id == lobbyId).FirstOrDefault();
            if (lobby is null)
                return null;

            var list = new List<SyncedScoreboardLobbyDataDto>();
            foreach (var player in lobby.Players.Values)
            {
                SyncedScoreboardLobbyDataDto entry = new SyncedScoreboardLobbyDataDto
                (
                    name: player.DisplayName,
                    playtimeMinutes: player.PlayMinutes,
                    kills: (int)((player.LobbyStats?.Kills ?? 0) + (player.CurrentRoundStats?.Kills ?? 0)),
                    assists: (int)((player.LobbyStats?.Assists ?? 0) + (player.CurrentRoundStats?.Assists ?? 0)),
                    deaths: player.LobbyStats?.Deaths ?? 0,
                    teamIndex: player.Team?.Entity.Index ?? 0
                );
                list.Add(entry);
            }
            return list;
        }

        private List<SyncedScoreboardMainmenuLobbyDataDto> GetDataForMainmenu()
        {
            List<SyncedScoreboardMainmenuLobbyDataDto> list = new List<SyncedScoreboardMainmenuLobbyDataDto>();
            foreach (ILobby lobby in _lobbiesHandler.Lobbies.Where(l => !GetIgnoreLobbyInScoreboard(l)))
            {
                int playerscount = lobby.Players.Count;
                string playersstr = string.Empty;
                if (playerscount > 0)
                    playersstr = string.Join(", ", lobby.Players.Values.Select(p => p.DisplayName).OrderBy(n => n));

                SyncedScoreboardMainmenuLobbyDataDto entry = new SyncedScoreboardMainmenuLobbyDataDto
                (
                    Id: (int)lobby.Id,
                    IsOfficial: lobby.IsOfficial,
                    CreatorName: lobby.CreatorName,
                    LobbyName: lobby.Name,
                    PlayersCount: playerscount,
                    PlayersStr: playersstr
                );
                list.Add(entry);
            }
            return list;
        }

        private bool GetIgnoreLobbyInScoreboard(ILobby lobby)
            => (lobby.Type == LobbyType.MapCreateLobby && lobby.IsOfficial)     // Dummy map create lobby
            || lobby.Type == LobbyType.CharCreateLobby;

        private bool GetShowAllLobbies(LobbyType myLobbyType)
            => myLobbyType == LobbyType.MainMenu || myLobbyType == LobbyType.CharCreateLobby;

        #endregion Private Methods
    }
}
