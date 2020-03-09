namespace TDS_Shared.Default
{
    public static class ToClientEvent
    {
        public const string AddMapToVoting = "s1";
        public const string AmountInFightSync = "s2";
        public const string ApplySuicideAnimation = "s3";

        public const string BombDetonated = "s4";
        public const string BombNotOnHand = "s5";
        public const string BombOnHand = "s6";
        public const string BombPlanted = "s7";

        public const string ClearTeamPlayers = "s8";
        public const string CountdownStart = "s9";
        public const string CreateCustomLobbyResponse = "s10";

        public const string Death = "s11";

        public const string ExplodeHead = "s86";

        public const string GetSupportRequestData = "s12";
        public const string GotoPositionByMapCreator = "s13";

        public const string HitOpponent = "s14";

        public const string JoinLobby = "s15";
        public const string JoinSameLobby = "s16";

        public const string LeaveCustomLobbyMenu = "s17";
        public const string LeaveSameLobby = "s18";
        public const string LoadApplicationDataForAdmin = "s19";
        public const string LoadMapFavourites = "s20";
        public const string LoadMapForMapCreator = "s21";
        public const string LoadMapNamesToLoadForMapCreator = "s22";
        public const string LoadOwnMapRatings = "s23";
        public const string LoadUserpanelData = "s24";

        public const string MapChange = "s25";
        public const string MapClear = "s26";
        public const string MapCreatorRequestAllObjectsForPlayer = "s27";
        public const string MapCreatorStartNewMap = "s28";
        public const string MapCreatorSyncAllObjects = "s29";
        public const string MapCreatorSyncData = "s30";
        public const string MapCreatorSyncFixLastId = "s31";
        public const string MapCreatorSyncNewObject = "s32";
        public const string MapCreatorSyncObjectPosition = "s33";
        public const string MapCreatorSyncObjectRemove = "s34";
        public const string MapsListRequest = "s35";
        public const string MapVotingSyncOnPlayerJoin = "s36";

        public const string PlayCustomSound = "s37";
        public const string PlayerGotBomb = "s38";
        public const string PlayerPlantedBomb = "s39";
        public const string PlayerSpectateMode = "s40";
        public const string PlayerJoinedTeam = "s41";
        public const string PlayerLeftTeam = "s42";
        public const string PlayerRespawned = "s43";
        public const string PlayerTeamChange = "s44";
        public const string PlayerWeaponChange = "s45";

        public const string RegisterLoginSuccessful = "s46";
        public const string RemoveCustomLobby = "s47";
        public const string RemoveForceStayAtPosition = "s84";
        public const string RequestNewMapsList = "s48";

        public const string RemoveSyncedPlayerDatas = "s49";
        public const string RoundStart = "s50";
        public const string RoundEnd = "s51";

        public const string SaveMapCreatorReturn = "s52";
        public const string SendMapCreatorReturn = "s53";
        public const string SetEntityInvincible = "s54";
        public const string SetForceStayAtPosition = "s83";
        public const string SetMapVotes = "s55";
        public const string SetPlayerData = "s56";
        public const string SetPlayerInvincible = "s57";
        public const string SetSupportRequestClosed = "s58";
        public const string SpectatorReattachCam = "s59";
        public const string StartRankingShowAfterRound = "s60";
        public const string StartRegisterLogin = "s61";
        public const string StopBombPlantDefuse = "s62";
        public const string StopMapVoting = "s63";
        public const string StopRoundStats = "s64";
        public const string StopSpectator = "s65";
        public const string SyncAllCustomLobbies = "s66";
        public const string SyncNewCustomLobby = "s67";
        public const string SyncNewSupportRequestMessage = "s68";
        public const string SyncPlayerData = "s69";
        public const string SyncSettings = "s70";
        public const string SyncScoreboardData = "s71";
        public const string SyncTeamChoiceMenuData = "s72";
        public const string SyncTeamPlayers = "s73";

        public const string ToggleTeamChoiceMenu = "s74";

        //public const string SyncPlayersSameLobby = "SyncPlayersSameLobby_Server";

        // Workarounds //
        public const string AttachEntityToEntityWorkaround = "s75";

        public const string DetachEntityWorkaround = "s76";
        public const string FreezeEntityWorkaround = "s85";
        public const string FreezePlayerWorkaround = "s77";
        public const string SetEntityCollisionlessWorkaround = "s78";
        public const string SetPlayerTeamWorkaround = "s79";
        public const string SetPlayerToSpectatePlayer = "s80";


        // Special //
        public const string ToBrowserEvent = "s81";
        public const string FromBrowserEventReturn = "s82";
    }
}
