using Newtonsoft.Json;
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

        [JsonIgnore]
        public string Json;

        public SyncedScoreboardLobbyDataDto(string name, uint playtimeMinutes, uint kills, uint assists, uint deaths, uint teamIndex)
        {
            Name = name;
            PlaytimeMinutes = playtimeMinutes;
            Kills = kills;
            Assists = assists;
            Deaths = deaths;
            TeamIndex = teamIndex;

            Json = JsonConvert.SerializeObject(this);
        }
    }
}