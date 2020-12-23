namespace TDS.Shared.Default
{
    public static class ToClientEvent
    {

        public const string AmountInFightSync = "s2";
        public const string ApplySuicideAnimation = "s3";

        // Workarounds //
        public const string AttachEntityToEntityWorkaround = "s75";

        public const string BombDetonated = "s4";
        public const string BombNotOnHand = "s5";
        public const string BombOnHand = "s6";
        public const string BombPlanted = "s7";

        public const string ClearTeamPlayers = "s8";
        public const string CountdownStart = "s9";
        public const string CreateCustomLobbyResponse = "s10";
        public const string CreateFreeGangHousesForLevel = "s88";

        public const string Death = "s11";

        //public const string SyncPlayersSameLobby = "SyncPlayersSameLobby_Server";
        public const string DetachEntityWorkaround = "s76";

        public const string ExplodeHead = "s86";

        public const string FreezeEntityWorkaround = "s85";
        public const string FreezePlayerWorkaround = "s77";
        public const string FromBrowserEventReturn = "s82";
        public const string GotoPositionByMapCreator = "s13";

        public const string HitOpponent = "s14";

        public const string JoinLobby = "s15";
        public const string JoinSameLobby = "s16";

        public const string LeaveSameLobby = "s18";
        public const string LoadMapForMapCreator = "s21";
        public const string LoadOwnMapRatings = "s23";

        public const string LoginSuccessful = "s46";
        public const string MapChange = "s25";
        public const string MapClear = "s26";
        public const string MapCreatorRequestAllObjectsForPlayer = "s27";
        public const string MapCreatorStartNewMap = "s28";
        public const string MapCreatorSyncAllObjects = "s29";
        public const string MapCreatorSyncFixLastId = "s31";
        public const string MapCreatorSyncNewObject = "s32";
        public const string MapCreatorSyncObjectPosition = "s33";
        public const string MapCreatorSyncObjectRemove = "s34";
        public const string MapCreatorSyncTeamObjectsRemove = "s1";
        public const string MapsListRequest = "s35";

        public const string PlayCustomSound = "s37";
        public const string PlayerGotBomb = "s38";
        public const string PlayerJoinedTeam = "s41";
        public const string PlayerLeftTeam = "s42";
        public const string PlayerPlantedBomb = "s39";
        public const string PlayerRespawned = "s43";
        public const string PlayerSpectateMode = "s40";
        public const string PlayerTeamChange = "s44";
        public const string PlayerWeaponChange = "s45";
        public const string RemoveForceStayAtPosition = "s84";
        public const string RemoveSyncedEntityDatas = "s17";
        public const string RemoveSyncedPlayerDatas = "s49";
        public const string RequestNewMapsList = "s48";
        public const string RoundEnd = "s51";
        public const string RoundStart = "s50";
        public const string SendAlert = "s89";
        public const string SetEntityCollisionlessWorkaround = "s78";
        public const string SetEntityData = "s12";
        public const string SetEntityInvincible = "s54";
        public const string SetForceStayAtPosition = "s83";
        public const string SetPlayerData = "s56";
        public const string SetPlayerInvincible = "s57";
        public const string SetPlayerTeamWorkaround = "s79";
        public const string SetPlayerToSpectatePlayer = "s80";
        public const string SpawnFakePickup = "s91";
        public const string SpectatorReattachCam = "s59";
        public const string StartCharCreator = "s20";
        public const string StartRankingShowAfterRound = "s60";
        public const string StopBombPlantDefuse = "s62";
        public const string SyncFakePickupLightData = "s92";
        public const string SyncFakePickups = "s94";
        public const string StopRoundStats = "s64";
        public const string StopSpectator = "s65";
        public const string SyncEntityData = "s19";
        public const string SyncPlayerCommandsSettings = "s87";
        public const string SyncPlayerData = "s69";
        public const string SyncScoreboardData = "s71";
        public const string SyncSettings = "s70";
        public const string SyncTeamChoiceMenuData = "s72";
        public const string SyncTeamPlayers = "s73";

        // Special //
        public const string ToBrowserEvent = "s81";

        public const string ToggleDamageTestMenu = "s90";
        public const string ToggleTeamChoiceMenu = "s74";

    }
}
