using GTANetworkAPI;
using Microsoft.EntityFrameworkCore.Internal;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.MapVotings;
using TDS.Server.Data.Models;
using TDS.Server.Data.Models.Map;
using TDS.Server.Data.Utility;
using TDS.Server.Handler.Events;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Maps;
using TDS.Server.LobbySystem.EventsHandlers;
using TDS.Shared.Core;
using TDS.Shared.Data.Models.Map;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.MapVotings
{
    public class ArenaMapVoting : IArenaMapVoting
    {
        private readonly List<MapVoteDto> _mapVotes = new List<MapVoteDto>();
        private readonly Dictionary<ITDSPlayer, int> _playerVotes = new Dictionary<ITDSPlayer, int>();
        private MapDto? _boughtMap = null;

        protected IArena Lobby { get; }
        private readonly MapsLoadingHandler _mapsLoadingHandler;
        private readonly ISettingsHandler _settingsHandler;
        private readonly IRoundFightLobbyEventsHandler _eventsHandler;
        private readonly RemoteBrowserEventsHandler _remoteBrowserEventsHandler;

        public ArenaMapVoting(IArena lobby, MapsLoadingHandler mapsLoadingHandler, ISettingsHandler settingsHandler, IRoundFightLobbyEventsHandler eventsHandler,
            RemoteBrowserEventsHandler remoteBrowserEventsHandler)
        {
            (Lobby, _mapsLoadingHandler, _settingsHandler, _eventsHandler, _remoteBrowserEventsHandler) = (lobby, mapsLoadingHandler, settingsHandler, eventsHandler, remoteBrowserEventsHandler);

            eventsHandler.RemoveAfter += EventsHandler_RemoveAfter;
            remoteBrowserEventsHandler.Add(ToServerEvent.BuyMap, BuyMap, player => player.Lobby == Lobby);
            remoteBrowserEventsHandler.Add(ToServerEvent.MapVote, MapVote, player => player.Lobby == Lobby);
        }

        private void EventsHandler_RemoveAfter(IBaseLobby lobby)
        {
            _eventsHandler.RemoveAfter -= EventsHandler_RemoveAfter;
            _remoteBrowserEventsHandler.Remove(ToServerEvent.BuyMap, BuyMap);
            _remoteBrowserEventsHandler.Remove(ToServerEvent.MapVote, MapVote);
        }

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

        private object? BuyMap(RemoteBrowserEventArgs args)
        {
            int? mapId;
            if ((mapId = Utils.GetInt(args.Args[0])) == null)
                return null;

            BuyMap(args.Player, mapId.Value);
            return null;
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

        private object? MapVote(RemoteBrowserEventArgs args)
        {
            if (args.Args.Count == 0)
                return null;

            int? mapId;
            if ((mapId = Utils.GetInt(args.Args[0])) == null)
                return null;

            VoteForMap(args.Player, mapId.Value);
            return null;
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

        public MapDto? GetBoughtMap()
        {
            if (_boughtMap is null)
                return null;

            var map = _boughtMap;
            _boughtMap = null;
            return map;
        }

        public MapDto? GetVotedMap()
        {
            if (_mapVotes.Count == 0)
                return null;

            MapVoteDto wonMap = _mapVotes.MaxBy(vote => vote.AmountVotes).First();
            Lobby.Notifications.Send(lang =>
            {
                return string.Format(lang.MAP_WON_VOTING, wonMap.Name);
            });
            lock (_mapVotes) { _mapVotes.Clear(); }
            lock (_playerVotes) { _playerVotes.Clear(); }
            return Lobby.MapHandler.Maps.FirstOrDefault(m => m.BrowserSyncedData.Id == wonMap.Id);
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