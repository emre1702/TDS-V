namespace TDS_Server_DB.Entity.Server
{
    public partial class ServerSettings
    {
        public short Id { get; set; }
        public string GamemodeName { get; set; }
        public bool ErrorToPlayerOnNonExistentCommand { get; set; }
        public bool ToChatOnNonExistentCommand { get; set; }
        public float DistanceToSpotToPlant { get; set; }
        public float DistanceToSpotToDefuse { get; set; }
        public int SavePlayerDataCooldownMinutes { get; set; }
        public int SaveLogsCooldownMinutes { get; set; }
        public int SaveSeasonsCooldownMinutes { get; set; }
        public int TeamOrderCooldownMs { get; set; }
        public float ArenaNewMapProbabilityPercent { get; set; }
        public int KillingSpreeMaxSecondsUntilNextKill { get; set; }
        public int MapRatingAmountForCheck { get; set; }
        public float MinMapRatingForNewMaps { get; set; }
        public float GiveMoneyFee { get; set; }
        public int GiveMoneyMinAmount { get; set; }
        public float NametagMaxDistance { get; set; }
        public bool ShowNametagOnlyOnAiming { get; set; }
        public int AFKKickAfterSec { get; set; }

        public float MultiplierRankingKills { get; set; }
        public float MultiplierRankingAssists { get; set; }
        public float MultiplierRankingDamage { get; set; }

        public int CloseApplicationAfterDays { get; set; }
        public int DeleteApplicationAfterDays { get; set; }
        public uint GangwarPreparationTimeMs { get; set; } 
        public uint GangwarActionTimeMs { get; set; } 
        public uint DeleteRequestsDaysAfterClose { get; set; }
        public int DeleteOfflineMessagesAfterDays { get; set; }

        public int MinPlayersOnlineForGangwar { get; set; }
        public int GangwarAreaAttackCooldownMinutes { get; set; }
        public int AmountPlayersAllowedInGangwarTeamBeforeCountCheck { get; set; }
        public bool GangwarAttackerCanBeMore { get; set; }
        public bool GangwarOwnerCanBeMore { get; set; }


        public int ReduceMapsBoughtCounterAfterMinute { get; set; }
        public int MapBuyBasePrice { get; set; }
        public float MapBuyCounterMultiplicator { get; set; }

        public int UsernameChangeCost { get; set; }
        public int UsernameChangeCooldownDays { get; set; }

        public int AmountWeeklyChallenges { get; set; }
    }
}
