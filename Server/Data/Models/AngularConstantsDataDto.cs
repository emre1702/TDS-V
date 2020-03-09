using Newtonsoft.Json;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Data.Models
{
    #nullable disable
    public class AngularConstantsDataDto
    {
        [JsonProperty("0")]
        public int TDSId { get; set; }

        [JsonProperty("1")]
        public ushort RemoteId { get; set; }

        [JsonProperty("2")]
        public int UsernameChangeCost { get; set; }

        [JsonProperty("3")]
        public int UsernameChangeCooldownDays { get; set; }

        [JsonProperty("4")]
        public int MapBuyBasePrice { get; set; }

        [JsonProperty("5")]
        public float MapBuyCounterMultiplicator { get; set; }

        [JsonProperty("6")]
        public string AnnouncementsJson { get; set; }

        [JsonProperty("7")]
        public string Username { get; set; }

        [JsonProperty("8")]
        public string SCName { get; set; }

        public static AngularConstantsDataDto Get(TDSPlayer player)
        {
            return new AngularConstantsDataDto
            {
                TDSId = player.Entity!.Id,
                RemoteId = player.Player.Handle.Value,
                UsernameChangeCost = SettingsManager.ServerSettings.UsernameChangeCost,
                UsernameChangeCooldownDays = SettingsManager.ServerSettings.UsernameChangeCooldownDays,
                MapBuyBasePrice = SettingsManager.ServerSettings.MapBuyBasePrice,
                MapBuyCounterMultiplicator = SettingsManager.ServerSettings.MapBuyCounterMultiplicator,
                AnnouncementsJson = AnnouncementsManager.Json,
                Username = player.Entity.Name,
                SCName = player.Entity.SCName
            };
        }
    }
    #nullable restore
}
