using Newtonsoft.Json;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Handler.Entities
{
#nullable disable

    public class AngularConstantsData
    {
        #region Public Properties

        [JsonProperty("6")]
        public string AnnouncementsJson { get; set; }

        [JsonProperty("4")]
        public int MapBuyBasePrice { get; set; }

        [JsonProperty("5")]
        public float MapBuyCounterMultiplicator { get; set; }

        [JsonProperty("0")]
        public int TDSId { get; set; }

        [JsonProperty("7")]
        public string Username { get; set; }

        [JsonProperty("3")]
        public int UsernameChangeCooldownDays { get; set; }

        [JsonProperty("2")]
        public int UsernameChangeCost { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static AngularConstantsData Get(ITDSPlayer player, ISettingsHandler settingsHandler, IAnnouncementsHandler announcementsHandler)
        {
            return new AngularConstantsData
            {
                TDSId = player.Id,
                UsernameChangeCost = settingsHandler.ServerSettings.UsernameChangeCost,
                UsernameChangeCooldownDays = settingsHandler.ServerSettings.UsernameChangeCooldownDays,
                MapBuyBasePrice = settingsHandler.ServerSettings.MapBuyBasePrice,
                MapBuyCounterMultiplicator = settingsHandler.ServerSettings.MapBuyCounterMultiplicator,
                AnnouncementsJson = announcementsHandler.Json,
                Username = player.Entity.Name,
            };
        }

        #endregion Public Methods
    }

#nullable restore
}
