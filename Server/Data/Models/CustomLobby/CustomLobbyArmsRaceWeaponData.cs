using GTANetworkAPI;
using Newtonsoft.Json;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Models.CustomLobby
{
    public class CustomLobbyArmsRaceWeaponData
    {
        [JsonProperty("0")]
        public WeaponHash? WeaponHash { get; set; }

        [JsonProperty("1")]
        public short AtKill { get; set; }
    }
}
