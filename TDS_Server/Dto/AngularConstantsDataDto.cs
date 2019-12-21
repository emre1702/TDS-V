using MessagePack;
using System.Collections.Generic;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Dto
{
    #nullable disable
    [MessagePackObject]
    public class AngularConstantsDataDto
    {
        [Key(0)]
        public int TDSId { get; set; }

        [Key(1)]
        public ushort RemoteId { get; set; }

        [Key(2)]
        public int UsernameChangeCost { get; set; }

        [Key(3)]
        public int UsernameChangeCooldownDays { get; set; }

        [Key(4)]
        public int MapBuyBasePrice { get; set; }

        [Key(5)]
        public float MapBuyCounterMultiplicator { get; set; }

        [Key(6)]
        public string AnnouncementsJson { get; set; }

        public static AngularConstantsDataDto Get(TDSPlayer player)
        {
            return new AngularConstantsDataDto
            {
                TDSId = player.Entity!.Id,
                RemoteId = player.Client.Handle.Value,
                UsernameChangeCost = SettingsManager.ServerSettings.UsernameChangeCost,
                UsernameChangeCooldownDays = SettingsManager.ServerSettings.UsernameChangeCooldownDays,
                MapBuyBasePrice = SettingsManager.ServerSettings.MapBuyBasePrice,
                MapBuyCounterMultiplicator = SettingsManager.ServerSettings.MapBuyCounterMultiplicator,
                AnnouncementsJson = AnnouncementsManager.Json
            };
        }
    }
    #nullable restore
}
