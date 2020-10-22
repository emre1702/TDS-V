using GTANetworkAPI;
using Newtonsoft.Json;

namespace TDS_Server.Data.Models
{
    public class DamageTestWeapon
    {
        [JsonProperty("0")]
        public WeaponHash Weapon { get; set; }

        [JsonProperty("1")]
        public float Damage { get; set; }

        [JsonProperty("2")]
        public float HeadshotDamageMultiplier { get; set; }
    }
}
