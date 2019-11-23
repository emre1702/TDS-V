using GTANetworkAPI;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        //private readonly Dictionary<string, uint> _mapVotes = new Dictionary<string, uint>();
        private readonly List<MapVoteDto> _mapVotes = new List<MapVoteDto>();
        private readonly Dictionary<Client, int> _playerVotes = new Dictionary<Client, int>();
        private MapDto? _boughtMap = null;

        public void SendMapsForVoting(Client player)
        {
            if (_mapsJson != null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.MapsListRequest, _mapsJson);
            }
        }

        public void MapVote(TDSPlayer player, int mapId)
        {
            if (_boughtMap is { })
                return;
            if (_playerVotes.ContainsKey(player.Client) && _playerVotes[player.Client] == mapId)
                return;

            if (_mapVotes.Any(m => m.Id == mapId))
            {
                RemovePlayerVote(player.Client);
                AddVoteToMap(player.Client, mapId);
                return;
            }

            if (_mapVotes.Count >= 9)
            {
                NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.NOT_MORE_MAPS_FOR_VOTING_ALLOWED);
                return;
            }

            MapDto? map = MapsLoader.GetMapById(mapId);
            if (map is null)
                map = MapCreator.GetMapById(mapId);
            if (map is null)
                return;

            RemovePlayerVote(player.Client);
            var mapVote = new MapVoteDto { Id = mapId, AmountVotes = 1, Name = map.Info.Name };
            _mapVotes.Add(mapVote);
            _playerVotes[player.Client] = mapId;
            SendAllPlayerEvent(DToClientEvent.AddMapToVoting, null, Serializer.ToBrowser(mapVote));
        }

        private void AddVoteToMap(Client player, int mapId)
        {
            _playerVotes[player] = mapId;
            var map = _mapVotes.First(m => m.Id == mapId);
            ++map.AmountVotes;
            SendAllPlayerEvent(DToClientEvent.SetMapVotes, null, mapId, map.AmountVotes);
        }

        private void RemovePlayerVote(Client player)
        {
            if (!_playerVotes.ContainsKey(player))
                return;

            int oldVote = _playerVotes[player];
            _playerVotes.Remove(player);

            MapVoteDto? oldVotedMap = _mapVotes.FirstOrDefault(m => m.Id == oldVote);
            if (oldVotedMap is null)
            {
                SendAllPlayerEvent(DToClientEvent.SetMapVotes, null, oldVote, 0);
                return;
            }
            if (--oldVotedMap.AmountVotes <= 0)
            {
                _mapVotes.RemoveAll(m => m.Id == oldVote);
                SendAllPlayerEvent(DToClientEvent.SetMapVotes, null, oldVote, 0);
                return;
            }
            SendAllPlayerEvent(DToClientEvent.SetMapVotes, null, oldVote, oldVotedMap.AmountVotes);
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
                    return Utils.GetReplaced(lang.MAP_WON_VOTING, wonMap.Name);
                });
                _mapVotes.Clear();
                _playerVotes.Clear();
                return _maps.FirstOrDefault(m => m.SyncedData.Id == wonMap.Id);
            }
            return null;
        }

        private void SyncMapVotingOnJoin(Client player)
        {
            if (_mapVotes.Count > 0)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.MapVotingSyncOnPlayerJoin, Serializer.ToBrowser(_mapVotes));
            }
        }

        public void BuyMap(TDSPlayer player, int mapId)
        {
            if (player.Entity is null)
                return;
            if (player.CurrentLobby is null)
                return;
            if (!player.IsLobbyOwner && !player.CurrentLobby.IsOfficial)
                return;

            MapDto? map = MapsLoader.GetMapById(mapId);
            if (map is null)
                map = MapCreator.GetMapById(mapId);
            if (map is null)
                return;

            if (!player.IsLobbyOwner)
            {
                int price = (int)Math.Ceiling(SettingsManager.ServerSettings.MapBuyBasePrice +
                            (SettingsManager.ServerSettings.MapBuyCounterMultiplicator * player.Entity.PlayerStats.MapsBoughtCounter));
                if (price > player.Money)
                {
                    NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.NOT_ENOUGH_MONEY);
                    return;
                }

                player.GiveMoney(-price);
                ++player.Entity.PlayerStats.MapsBoughtCounter;
                PlayerDataSync.SetData(player, EPlayerDataKey.MapsBoughtCounter, EPlayerDataSyncMode.Player, player.Entity.PlayerStats.MapsBoughtCounter);
            }

            _boughtMap = map;
            _mapVotes.Clear();
            _playerVotes.Clear();

            SendAllPlayerLangNotification(lang => string.Format(lang.MAP_BUY_INFO, player.Client.Name, map.SyncedData.Name));
            SendAllPlayerEvent(DToClientEvent.StopMapVoting, null);
        }
    }
}