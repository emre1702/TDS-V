using GTANetworkAPI;
using MoreLinq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Common.Dto;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        //private readonly Dictionary<string, uint> _mapVotes = new Dictionary<string, uint>();
        private readonly List<MapVoteDto> _mapVotes = new List<MapVoteDto>();
        private readonly Dictionary<Client, int> _playerVotes = new Dictionary<Client, int>();

        public void SendMapsForVoting(Client player)
        {
            if (_mapsJson != null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.MapsListRequest, _mapsJson);
            }
        }

        public void MapVote(TDSPlayer player, int mapId)
        {
            if (_mapVotes.Any(m => m.Id == mapId))
            {
                AddVoteToMap(player.Client, mapId);
                return;
            }

            if (_mapVotes.Count >= 9)
            {
                NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.NOT_MORE_MAPS_FOR_VOTING_ALLOWED);
                return;
            }
            MapDto? map = MapsLoader.GetMapById(mapId);
            if (map == null)
                map = MapCreator.GetMapById(mapId);
            if (map == null)
                return;
            var mapVote = new MapVoteDto { Id = mapId, AmountVotes = 1, Name = map.Info.Name };
            _mapVotes.Add(mapVote);
            SendAllPlayerEvent(DToClientEvent.AddMapToVoting, null, JsonConvert.SerializeObject(mapVote));
        }

        private void AddVoteToMap(Client player, int mapId)
        {
            if (_playerVotes.ContainsKey(player))
            {
                int oldVote = _playerVotes[player];
                _playerVotes.Remove(player);

                MapVoteDto? oldVotedMap = _mapVotes.FirstOrDefault(m => m.Id == oldVote);
                if (oldVotedMap != null && --oldVotedMap.AmountVotes <= 0)
                {
                    _mapVotes.RemoveAll(m => m.Id == oldVote);
                    SendAllPlayerEvent(DToClientEvent.RemoveMapFromVoting, null, oldVote);
                    SendAllPlayerEvent(DToClientEvent.AddVoteToMap, null, mapId);
                }
                else
                    SendAllPlayerEvent(DToClientEvent.AddVoteToMap, null, mapId, oldVote);
            }
            else
                SendAllPlayerEvent(DToClientEvent.AddVoteToMap, null, mapId);
            _playerVotes[player] = mapId;
            ++_mapVotes.First(m => m.Id == mapId).AmountVotes;
        }

        private MapDto? GetVotedMap()
        {
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
                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.MapVotingSyncOnPlayerJoin, JsonConvert.SerializeObject(_mapVotes));
            }
        }
    }
}