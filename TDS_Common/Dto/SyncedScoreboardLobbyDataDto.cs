using Newtonsoft.Json;
using System;

namespace TDS_Common.Dto
{
    [Serializable]
    public class SyncedScoreboardLobbyDataDto
    {
        public string Name;
        public int PlaytimeMinutes;
        public int Kills;
        public int Assists;
        public int Deaths;
        public int TeamIndex;

        [JsonIgnore]
        public string Json;

        public SyncedScoreboardLobbyDataDto(string name, int playtimeMinutes, int kills, int assists, int deaths, int teamIndex)
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