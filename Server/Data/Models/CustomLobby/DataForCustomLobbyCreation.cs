using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS_Server.Data.Models.CustomLobby
{
    public class DataForCustomLobbyCreation
    {
#nullable disable
        [JsonProperty("0")]
        public List<CustomLobbyWeaponData> WeaponDatas { get; set; }

        [JsonProperty("1")]
        public List<CustomLobbyWeaponData> ArenaWeaponDatas { get; set; }

        [JsonProperty("2")]
        public List<CustomLobbyArmsRaceWeaponData> ArenaArmsRaceWeaponDatas { get; set; }
    }
}
