﻿using Newtonsoft.Json;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;

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

        [JsonProperty("1")]
        public ushort RemoteId { get; set; }

        [JsonProperty("8")]
        public string SCName { get; set; }

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
                RemoteId = player.RemoteId,
                UsernameChangeCost = settingsHandler.ServerSettings.UsernameChangeCost,
                UsernameChangeCooldownDays = settingsHandler.ServerSettings.UsernameChangeCooldownDays,
                MapBuyBasePrice = settingsHandler.ServerSettings.MapBuyBasePrice,
                MapBuyCounterMultiplicator = settingsHandler.ServerSettings.MapBuyCounterMultiplicator,
                AnnouncementsJson = announcementsHandler.Json,
                Username = player.Entity.Name,
                SCName = player.Entity.SCName
            };
        }

        #endregion Public Methods
    }

#nullable restore
}