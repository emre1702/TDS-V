using Newtonsoft.Json;
using System;

namespace TDS_Common.Dto
{
    [Serializable]
    public class SyncedScoreboardMainmenuLobbyDataDto
    {
        public int Id;
        public string LobbyName;
        public bool IsOfficial;
        public string CreatorName;
        public string PlayersStr;
        public int PlayersCount;

        public SyncedScoreboardMainmenuLobbyDataDto(int Id, string LobbyName, bool IsOfficial, string CreatorName, string PlayersStr, int PlayersCount)
        {
            this.Id = Id;
            this.LobbyName = LobbyName;
            this.IsOfficial = IsOfficial;
            this.CreatorName = CreatorName;
            this.PlayersStr = PlayersStr;
            this.PlayersCount = PlayersCount;
        }
    }
}