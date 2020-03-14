using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Models.CustomLobby
{
    #nullable disable
    public class CustomLobbyData
    {
        [JsonProperty("0")]
        public int? LobbyId { get; set; }
        [JsonProperty("1")]
        public string Name { get; set; } = "";
        [JsonProperty("2")]
        public string OwnerName { get; set; }
        [JsonProperty("3")]
        public string Password { get; set; } = "";
        [JsonProperty("8")]
        public bool ShowRanking { get; set; }
        [JsonProperty("4")]
        public short StartHealth { get; set; }
        [JsonProperty("5")]
        public short StartArmor { get; set; }
        [JsonProperty("6")]
        public short AmountLifes { get; set; }
        [JsonProperty("7")]
        public bool MixTeamsAfterRound { get; set; }
        [JsonProperty("9")]
        public int BombDetonateTimeMs { get; set; }
        [JsonProperty("10")]
        public int BombDefuseTimeMs { get; set; }
        [JsonProperty("11")]
        public int BombPlantTimeMs { get; set; }
        [JsonProperty("12")]
        public int RoundTime { get; set; }
        [JsonProperty("13")]
        public int CountdownTime { get; set; }
        [JsonProperty("14")]
        public int SpawnAgainAfterDeathMs { get; set; }
        [JsonProperty("15")]
        public int MapLimitTime { get; set; }
        [JsonProperty("16")]
        public EMapLimitType MapLimitType { get; set; }
        [JsonProperty("17")]
        public List<CustomLobbyTeamData> Teams { get; set; }
        [JsonProperty("18")]
        public List<int> Maps { get; set; }
        [JsonProperty("19")]
        public List<CustomLobbyWeaponData> Weapons { get; set; }
    }
    #nullable restore
}
