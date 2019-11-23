using MessagePack;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class SyncedScoreboardMainmenuLobbyDataDto
    {
        [Key(0)]
        public int Id;
        [Key(1)]
        public string LobbyName;
        [Key(2)]
        public bool IsOfficial;
        [Key(3)]
        public string CreatorName;
        [Key(4)]
        public string PlayersStr;
        [Key(5)]
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