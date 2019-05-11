using GTANetworkAPI;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Default;
using TDS_Server.Dto.Map;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class Arena
    {
        private readonly Dictionary<string, uint> _mapVotes = new Dictionary<string, uint>();
        private readonly Dictionary<Client, string> _playerVotes = new Dictionary<Client, string>();

        public void SendMapsForVoting(Client player)
        {
            if (_mapsJson != null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.MapsListRequest, _mapsJson);
            }
        }

        public void MapVote(TDSPlayer player, string mapname)
        {
            if (!_mapVotes.ContainsKey(mapname))
            {
                if (_mapVotes.Count >= 9)
                {
                    NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.NOT_MORE_MAPS_FOR_VOTING_ALLOWED);
                    return;
                }
                _mapVotes[mapname] = 0;
            }
            AddVoteToMap(player.Client, mapname);
        }

        private void AddVoteToMap(Client player, string mapname)
        {
            if (_playerVotes.ContainsKey(player))
            {
                string oldvote = _playerVotes[player];
                _playerVotes.Remove(player);

                if (oldvote == mapname)
                    return;
                if (--_mapVotes[oldvote] <= 0)
                {
                    _mapVotes.Remove(oldvote);
                }
                SendAllPlayerEvent(DToClientEvent.AddVoteToMap, null, mapname, oldvote);
            }
            else
                SendAllPlayerEvent(DToClientEvent.AddVoteToMap, null, mapname);
            _playerVotes[player] = mapname;
            _mapVotes[mapname]++;
        }

        private MapDto? GetVotedMap()
        {
            if (_mapVotes.Count > 0)
            {
                string wonmap = _mapVotes.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                SendAllPlayerLangNotification(lang =>
                {
                    return Utils.GetReplaced(lang.MAP_WON_VOTING, wonmap);
                });
                _mapVotes.Clear();
                _playerVotes.Clear();
                return _maps.FirstOrDefault(m => m.Info.Name == wonmap);
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