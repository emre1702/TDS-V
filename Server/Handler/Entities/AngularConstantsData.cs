﻿using Newtonsoft.Json;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities
{
#nullable disable
    public class AngularConstantsData
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

        public static AngularConstantsData Get(ITDSPlayer player)
        {
            return new AngularConstantsData
            {
                TDSId = player.Id,
                RemoteId = player.RemoteId,
                UsernameChangeCost = _settingsHandler.ServerSettings.UsernameChangeCost,
                UsernameChangeCooldownDays = _settingsHandler.ServerSettings.UsernameChangeCooldownDays,
                MapBuyBasePrice = _settingsHandler.ServerSettings.MapBuyBasePrice,
                MapBuyCounterMultiplicator = _settingsHandler.ServerSettings.MapBuyCounterMultiplicator,
                AnnouncementsJson = _announcementsHandler.Json,
                Username = player.Entity.Name,
                SCName = player.Entity.SCName
            };
        }
    }
#nullable restore
}