using Newtonsoft.Json;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Models.CustomLobby
{
    public class CustomLobbyWeaponData
    {
        #region Public Properties

        [JsonProperty("1")]
        public int Ammo { get; set; }

        [JsonProperty("2")]
        public float? Damage { get; set; }

        [JsonProperty("3")]
        public float? HeadshotMultiplicator { get; set; }

        [JsonProperty("0")]
        public WeaponHash WeaponHash { get; set; }

        #endregion Public Properties

        /*[JsonProperty("4")]
        public float MinHeadshotDistance { get; set; }

        [JsonProperty("5")]
        public float MaxHeadshotDistance { get; set; }*/
    }
}
