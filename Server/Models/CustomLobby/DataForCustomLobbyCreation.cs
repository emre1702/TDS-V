using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS_Server.Dto.CustomLobby
{
    public class DataForCustomLobbyCreation
    {
        #nullable disable
        [JsonProperty("0")]
        public List<CustomLobbyWeaponData> WeaponDatas { get; set; }

        [JsonProperty("1")]
        public List<CustomLobbyWeaponData> ArenaWeaponDatas { get; set; }
    }
}
