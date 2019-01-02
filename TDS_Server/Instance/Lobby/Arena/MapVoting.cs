namespace TDS_Server.Instance.Lobby
{
    using GTANetworkAPI;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using TDS_Server.Dto;
    using TDS_Server.Manager.Utility;
    using TDS_Common.Default;
    using TDS_Server.Instance.Player;

    partial class Arena
    {

        private readonly Dictionary<string, uint> mapVotes = new Dictionary<string, uint>();
        private readonly Dictionary<Client, string> playerVotes = new Dictionary<Client, string>();

        public void SendMapsForVoting(Client player)
        {
            if (mapsJson != null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.MapsListRequest, mapsJson);
            }
        }

        public void AddVoteToMap(Client player, string mapname)
        {
            if (playerVotes.ContainsKey(player))
            {
                string oldvote = playerVotes[player];
                playerVotes.Remove(player);

                if (oldvote == mapname)
                    return;
                if (--mapVotes[oldvote] <= 0)
                {
                    mapVotes.Remove(oldvote, out uint _);
                }
                SendAllPlayerEvent(DToClientEvent.AddVoteToMap, null, mapname, oldvote);
            }
            else
                SendAllPlayerEvent(DToClientEvent.AddVoteToMap, null, mapname);
            playerVotes[player] = mapname;
            mapVotes[mapname]++;
        }

        public void AddMapToVoting(TDSPlayer player, string mapname)
        {
            if (!mapVotes.ContainsKey(mapname))
            {
                if (mapVotes.Count < 9)
                {
                    mapVotes[mapname] = 0;
                    AddVoteToMap(player.Client, mapname);
                }
                else
                    NAPI.Notification.SendNotificationToPlayer(player.Client, player.Language.NOT_MORE_MAPS_FOR_VOTING_ALLOWED);
            }
        }

        private MapDto GetVotedMap()
        {
            if (mapVotes.Count > 0)
            {
                string wonmap = mapVotes.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                SendAllPlayerLangNotification(lang =>
                {
                    return Utils.GetReplaced(lang.MAP_WON_VOTING, wonmap);
                });
                mapVotes.Clear();
                playerVotes.Clear();
                for (int i = 0; i < maps.Count; ++i)
                    if (maps[i].SyncedData.Name == wonmap)
                        return maps[i];
            }
            return null;
        }

        private void SyncMapVotingOnJoin(Client player)
        {
            if (mapVotes.Count > 0)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.MapVotingSyncOnPlayerJoin, JsonConvert.SerializeObject(mapVotes));
            }
        }

        /*
		

        

		

		*/
    }

}
