using System;

namespace TDS_Common.Dto
{
    [Serializable]
    public class SyncedScoreboardLobbyDataDto
    {
        public string Name;
        public uint PlaytimeMinutes;
        public uint Kills;
        public uint Assists;
        public uint Deaths;
        public uint TeamIndex;
    }
}
