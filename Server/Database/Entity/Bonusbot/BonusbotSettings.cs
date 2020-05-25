namespace TDS_Server.Database.Entity.Bonusbot
{
    public class BonusbotSettings
    {
        #region Public Properties

        public ulong? ActionsInfoChannelId { get; set; }
        public ulong? AdminApplicationsChannelId { get; set; }
        public ulong? BansInfoChannelId { get; set; }
        public ulong? ErrorLogsChannelId { get; set; }
        public ulong? GuildId { get; set; }
        public int Id { get; set; }
        public int RefreshServerStatsFrequencySec { get; set; }
        public bool SendPrivateMessageOnBan { get; set; }
        public bool SendPrivateMessageOnOfflineMessage { get; set; }
        public ulong? ServerInfosChannelId { get; set; }
        public ulong? SupportRequestsChannelId { get; set; }

        #endregion Public Properties
    }
}
