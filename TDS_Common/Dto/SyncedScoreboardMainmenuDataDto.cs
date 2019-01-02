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
    }
}
