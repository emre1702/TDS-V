namespace TDS_Server_DB.Entity.Bonusbot
{
    public class BonusbotSettings
    {
        public int Id { get; set; }

        public ulong? GuildId { get; set; }

        public ulong? AdminApplicationsChannelId { get; set; }
        public ulong? ServerInfosChannelId { get; set; }
        public ulong? SupportRequestsChannelId { get; set; }
        public ulong? ActionsInfoChannelId { get; set; }

        public ulong? ErrorLogsChannelId { get; set; }

        public bool SendPrivateMessageOnBan { get; set; }
        public bool SendPrivateMessageOnOfflineMessage { get; set; }

        public int RefreshServerStatsFrequencySec { get; set; }
    }
}
