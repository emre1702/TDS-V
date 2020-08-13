using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using TDS_Shared.Data.Models.Map;
using TDS_Shared.Default;

namespace TDS_Server.Entity.LobbySystem.ArenaSystem
{
    partial class Arena
    {
        #region Private Fields

        //private readonly Dictionary<string, uint> _mapVotes = new Dictionary<string, uint>();
        private readonly List<MapVoteDto> _mapVotes = new List<MapVoteDto>();

        private readonly Dictionary<ITDSPlayer, int> _playerVotes = new Dictionary<ITDSPlayer, int>();
        private MapDto? _boughtMap = null;

        #endregion Private Fields

        #region Public Methods

        public void BuyMap(ITDSPlayer player, int mapId)
        {
            if (player.Entity is null)
                return;
            if (player.Lobby is null)
                return;
            if (!player.IsLobbyOwner && !player.Lobby.IsOfficial)
                return;

            MapDto? map = _mapsLoadingHandler.GetMapById(mapId);
            if (map is null)
                return;

            if (!player.IsLobbyOwner)
            {
                int price = (int)Math.Ceiling(SettingsHandler.ServerSettings.MapBuyBasePrice +
                            (SettingsHandler.ServerSettings.MapBuyCounterMultiplicator * player.Entity.PlayerStats.MapsBoughtCounter));
                if (price > player.Money)
                {
                    player.SendNotification(player.Language.NOT_ENOUGH_MONEY);
                    return;
                }

                player.GiveMoney(-price);
                ++player.Entity.PlayerStats.MapsBoughtCounter;
                if (player.LobbyStats is { })
                    ++player.LobbyStats.TotalMapsBought;
                player.AddToChallenge(ChallengeType.BuyMaps);
                DataSyncHandler.SetData(player, PlayerDataKey.MapsBoughtCounter, DataSyncMode.Player, player.Entity.PlayerStats.MapsBoughtCounter);
            }

            _boughtMap = map;
            _mapVotes.Clear();
            _playerVotes.Clear();

            SendAllPlayerLangNotification(lang => string.Format(lang.MAP_BUY_INFO, player.DisplayName, map.BrowserSyncedData.Name));
            ModAPI.Sync.SendEvent(this, ToClientEvent.ToBrowserEvent, ToBrowserEvent.StopMapVoting);
        }

        public void MapVote(ITDSPlayer player, int mapId)
        {
            if (_boughtMap is { })
                return;
            if (_playerVotes.ContainsKey(player) && _playerVotes[player] == mapId)
                return;

            if (_mapVotes.Any(m => m.Id == mapId))
            {
                RemovePlayerVote(player);
                AddVoteToMap(player, mapId);
                return;
            }

            if (_mapVotes.Count >= 9)
            {
                player.SendNotification(player.Language.NOT_MORE_MAPS_FOR_VOTING_ALLOWED);
                return;
            }

            MapDto? map = _mapsLoadingHandler.GetMapById(mapId);
            if (map is null)
                return;

            RemovePlayerVote(player);
            var mapVote = new MapVoteDto { Id = mapId, AmountVotes = 1, Name = map.Info.Name };
            _mapVotes.Add(mapVote);
            _playerVotes[player] = mapId;
            ModAPI.Sync.SendEvent(this, ToClientEvent.ToBrowserEvent, ToBrowserEvent.AddMapToVoting, Serializer.ToBrowser(mapVote));
        }

        public void SendMapsForVoting(ITDSPlayer player)
        {
            if (_mapsJson != null)
            {
                player.SendEvent(ToClientEvent.MapsListRequest, _mapsJson);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void AddVoteToMap(ITDSPlayer player, int mapId)
        {
            _playerVotes[player] = mapId;
            var map = _mapVotes.First(m => m.Id == mapId);
            ++map.AmountVotes;
            ModAPI.Sync.SendEvent(this, ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetMapVotes, mapId, map.AmountVotes);
        }

        private MapDto? GetVotedMap()
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
                SendAllPlayerLangNotification(lang =>
                {
                    return string.Format(lang.MAP_WON_VOTING, wonMap.Name);
                });
                _mapVotes.Clear();
                _playerVotes.Clear();
                return _maps.FirstOrDefault(m => m.BrowserSyncedData.Id == wonMap.Id);
            }
            return null;
        }

        private void RemovePlayerVote(ITDSPlayer player)
        {
            if (!_playerVotes.ContainsKey(player))
                return;

            int oldVote = _playerVotes[player];
            _playerVotes.Remove(player);

            MapVoteDto? oldVotedMap = _mapVotes.FirstOrDefault(m => m.Id == oldVote);
            if (oldVotedMap is null)
            {
                ModAPI.Sync.SendEvent(this, ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetMapVotes, oldVote, 0);
                return;
            }
            if (--oldVotedMap.AmountVotes <= 0)
            {
                _mapVotes.RemoveAll(m => m.Id == oldVote);
                ModAPI.Sync.SendEvent(this, ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetMapVotes, oldVote, 0);
                return;
            }
            ModAPI.Sync.SendEvent(this, ToClientEvent.ToBrowserEvent, ToBrowserEvent.SetMapVotes, oldVote, oldVotedMap.AmountVotes);
        }

        private void SyncMapVotingOnJoin(ITDSPlayer player)
        {
            if (_mapVotes.Count > 0)
            {
                player.SendEvent(ToClientEvent.ToBrowserEvent, ToBrowserEvent.LoadMapVoting, Serializer.ToBrowser(_mapVotes));
            }
        }

        #endregion Private Methods
    }
}
