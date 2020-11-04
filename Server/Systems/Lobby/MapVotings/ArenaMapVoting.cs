using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Internal;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.MapVotings;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.Maps;
using TDS_Shared.Core;
using TDS_Shared.Data.Models.Map;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.MapVotings
{
    public class ArenaMapVoting : IArenaMapVoting
    {
        private readonly List<MapVoteDto> _mapVotes = new List<MapVoteDto>();
        private readonly Dictionary<ITDSPlayer, int> _playerVotes = new Dictionary<ITDSPlayer, int>();
        private MapDto? _boughtMap = null;

        protected IArena Lobby { get; }
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly ISettingsHandler _settingsHandler;

        public ArenaMapVoting(IArena lobby, MapsLoadingHandler mapsLoadingHandler, ISettingsHandler settingsHandler)
            => (Lobby, _mapsLoadingHandler, _settingsHandler) = (lobby, mapsLoadingHandler, settingsHandler);

        public void BuyMap(ITDSPlayer player, int mapId)
        {
            if (!CheckCanBuyMap(player))
                return;
            var map = _mapsLoadingHandler.GetMapById(mapId);
            if (map is null)
                return;

            if (!player.IsLobbyOwner)
                SetPlayerBoughtMap(player);

            _boughtMap = map;
            lock (_mapVotes) { _mapVotes.Clear(); }
            lock (_playerVotes) { _playerVotes.Clear(); }

            Lobby.Notifications.Send(lang => string.Format(lang.MAP_BUY_INFO, player.DisplayName, map.BrowserSyncedData.Name));
            NAPI.Task.RunSafe(() =>
                Lobby.Sync.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.StopMapVoting));
        }

        public void VoteForMap(ITDSPlayer player, int mapId)
        {
            if (!CheckCanVoteForMap(player, mapId))
                return;

            if (DoVotesForMapIdExist(mapId))
            {
                RemovePlayerVote(player);
                AddVoteToMap(player, mapId);
                return;
            }

            lock (_mapVotes)
            {
                if (_mapVotes.Count >= 9)
                {
                    NAPI.Task.RunSafe(() => player.SendNotification(player.Language.NOT_MORE_MAPS_FOR_VOTING_ALLOWED));
                    return;
                }
            }

            var map = _mapsLoadingHandler.GetMapById(mapId);
            if (map is null)
                return;
            RemovePlayerVote(player);
            SetVoteToMap(player, map);
        }

        public string? GetJson()
        {
            string json;
            lock (_mapVotes) { json = Serializer.ToBrowser(_mapVotes); }
            return json;
        }

        private void SetPlayerBoughtMap(ITDSPlayer player)
        {
            int price = GetMapBuyPrice(player);
            if (price > player.Money)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.NOT_ENOUGH_MONEY));
                return;
            }
            player.MapsVoting.SetBoughtMap(price);
        }

        private void AddVoteToMap(ITDSPlayer player, int mapId)
        {
            lock (_playerVotes) { _playerVotes[player] = mapId; }
            MapVoteDto _mapVoteDto;
            lock (_mapVotes) { _mapVoteDto = _mapVotes.First(m => m.Id == mapId); }
            ++_mapVoteDto.AmountVotes;
            Lobby.Sync.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetMapVotes, mapId, _mapVoteDto.AmountVotes);
        }

        private void SetVoteToMap(ITDSPlayer player, MapDto map)
        {
            var mapVote = new MapVoteDto { Id = map.BrowserSyncedData.Id, AmountVotes = 1, Name = map.Info.Name };
            lock (_mapVotes) { _mapVotes.Add(mapVote); }
            lock (_playerVotes) { _playerVotes[player] = map.BrowserSyncedData.Id; }
            Lobby.Sync.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.AddMapToVoting, Serializer.ToBrowser(mapVote));
        }

        private void RemovePlayerVote(ITDSPlayer player)
        {
            int oldVote;
            lock (_playerVotes)
            {
                if (!_playerVotes.ContainsKey(player))
                    return;
                oldVote = _playerVotes[player];
                _playerVotes.Remove(player);
            }

            MapVoteDto? oldVotedMap;
            lock (_mapVotes)
            {
                oldVotedMap = _mapVotes.FirstOrDefault(m => m.Id == oldVote);
            }
            if (oldVotedMap is null)
            {
                Lobby.Sync.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetMapVotes, oldVote, 0);
                return;
            }
            if (--oldVotedMap.AmountVotes <= 0)
            {
                lock (_mapVotes) { _mapVotes.RemoveAll(m => m.Id == oldVote); }
                Lobby.Sync.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetMapVotes, oldVote, 0);
                return;
            }
            Lobby.Sync.TriggerEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetMapVotes, oldVote, oldVotedMap.AmountVotes);
        }

        public MapDto? GetVotedMap()
        {
            if (_boughtMap is { })
            {
                var map = _boughtMap;
                _boughtMap = null;
                return map;
            }
            if (_mapVotes.Count > 0)
            {
                MapVoteDto wonMap = _mapVotes.MaxBy(vote => vote.AmountVotes).First();
                Lobby.Notifications.Send(lang =>
                {
                    return string.Format(lang.MAP_WON_VOTING, wonMap.Name);
                });
                lock (_mapVotes) { _mapVotes.Clear(); }
                lock (_playerVotes) { _playerVotes.Clear(); }
                return Lobby.MapHandler.Maps.FirstOrDefault(m => m.BrowserSyncedData.Id == wonMap.Id);
            }
            return null;
        }

        private bool CheckCanBuyMap(ITDSPlayer player)
        {
            if (player.Entity is null)
                return false;
            if (_boughtMap is { })
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.A_MAP_WAS_ALREADY_BOUGHT));
                return false;
            }

            if (!player.IsLobbyOwner && !Lobby.IsOfficial)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.YOU_CANT_BUY_A_MAP_IN_CUSTOM_LOBBY));
                return false;
            }

            return true;
        }

        private bool CheckCanVoteForMap(ITDSPlayer player, int mapId)
        {
            if (_boughtMap is { })
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.A_MAP_WAS_ALREADY_BOUGHT));
                return false;
            }
            lock (_playerVotes)
            {
                if (_playerVotes.TryGetValue(player, out var votedMapId) && votedMapId == mapId)
                    return false;
            }
            return true;
        }

        private int GetMapBuyPrice(ITDSPlayer player)
            => (int)Math.Ceiling(_settingsHandler.ServerSettings.MapBuyBasePrice +
                            (_settingsHandler.ServerSettings.MapBuyCounterMultiplicator * player.Entity!.PlayerStats.MapsBoughtCounter));

        private bool DoVotesForMapIdExist(int mapId)
        {
            lock (_mapVotes) { return _mapVotes.Any(v => v.Id == mapId); }
        }
    }
}
