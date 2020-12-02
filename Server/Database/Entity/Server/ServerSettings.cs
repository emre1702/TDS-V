namespace TDS.Server.Database.Entity.Server
{
    public partial class ServerSettings
    {
        public int AmountPlayersAllowedInGangwarTeamBeforeCountCheck { get; set; }
        public int AmountWeeklyChallenges { get; set; }
        public float ArenaNewMapProbabilityPercent { get; set; }
        public int CloseApplicationAfterDays { get; set; }
        public int DeleteApplicationAfterDays { get; set; }
        public int DeleteOfflineMessagesAfterDays { get; set; }
        public uint DeleteRequestsDaysAfterClose { get; set; }
        public float DistanceToSpotToDefuse { get; set; }
        public float DistanceToSpotToPlant { get; set; }
        public bool ErrorToPlayerOnNonExistentCommand { get; set; }
        public string GamemodeName { get; set; }
        public uint GangActionRoundTime { get; set; }
        public int GangActionAreaAttackCooldownMinutes { get; set; }
        public bool GangActionAttackerCanBeMore { get; set; }
        public bool GangActionOwnerCanBeMore { get; set; }
        public uint GangActionPreparationTime { get; set; }
        public double GangActionTargetRadius { get; set; }
        public int GangActionTargetWithoutAttackerMaxSeconds { get; set; }
        public float GiveMoneyFee { get; set; }
        public int GiveMoneyMinAmount { get; set; }
        public short Id { get; set; }
        public int KillingSpreeMaxSecondsUntilNextKill { get; set; }
        public int MapBuyBasePrice { get; set; }
        public float MapBuyCounterMultiplicator { get; set; }
        public int MapRatingAmountForCheck { get; set; }
        public float MinMapRatingForNewMaps { get; set; }
        public int MinPlayersOnlineForGangAction { get; set; }
        public float MultiplierRankingAssists { get; set; }
        public float MultiplierRankingDamage { get; set; }
        public float MultiplierRankingKills { get; set; }
        public float NametagMaxDistance { get; set; }
        public int ReduceMapsBoughtCounterAfterMinute { get; set; }
        public int ReloadServerBansEveryMinutes { get; set; }
        public int SaveLogsCooldownMinutes { get; set; }
        public int SavePlayerDataCooldownMinutes { get; set; }
        public int SaveSeasonsCooldownMinutes { get; set; }
        public bool ShowNametagOnlyOnAiming { get; set; }
        public int TeamOrderCooldownMs { get; set; }
        public bool ToChatOnNonExistentCommand { get; set; }
        public int UsernameChangeCooldownDays { get; set; }
        public int UsernameChangeCost { get; set; }
        public byte AmountCharSlots { get; set; }
        public string GitHubRepoOwnerName { get; set; }
        public string GitHubRepoRepoName { get; set; }
        public int GangMaxGangActionAttacksPerDay { get; set; }
        public int MapCreatorRewardRandomlySelected { get; set; }
        public int MapCreatorRewardVoted { get; set; }
        public int MapCreatorRewardBought { get; set; }
    }
}
