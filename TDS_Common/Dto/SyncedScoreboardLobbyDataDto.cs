using MessagePack;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class SyncedScoreboardLobbyDataDto
    {
        [Key(0)]
        public string Name;
        [Key(1)]
        public int PlaytimeMinutes;
        [Key(2)]
        public int Kills;
        [Key(3)]
        public int Assists;
        [Key(4)]
        public int Deaths;
        [Key(5)]
        public int TeamIndex;

        public SyncedScoreboardLobbyDataDto(string name, int playtimeMinutes, int kills, int assists, int deaths, int teamIndex)
        {
            Name = name;
            PlaytimeMinutes = playtimeMinutes;
            Kills = kills;
            Assists = assists;
            Deaths = deaths;
            TeamIndex = teamIndex;
        }
    }
}