using GTANetworkAPI;
using Newtonsoft.Json;

namespace TDS_Server.Dto.CustomLobby
{
    public class CustomLobbyWeaponData
    {
        [JsonProperty("0")]
        public WeaponHash WeaponHash { get; set; }

        [JsonProperty("1")]
        public int Ammo { get; set; }

        [JsonProperty("2")]
        public float? Damage { get; set; }

        [JsonProperty("3")]
        public float? HeadshotMultiplicator { get; set; }
        
        /*[JsonProperty("4")]
        public float MinHeadshotDistance { get; set; }

        [JsonProperty("5")]
        public float MaxHeadshotDistance { get; set; }*/
    }
}
