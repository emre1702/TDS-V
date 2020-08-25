enum ToClientEvent {
    AmountInFightSync = "s2",
    ApplySuicideAnimation = "s3",

    // Workarounds //
    AttachEntityToEntityWorkaround = "s75",

    BombDetonated = "s4",
    BombNotOnHand = "s5",
    BombOnHand = "s6",
    BombPlanted = "s7",

    ClearTeamPlayers = "s8",
    CountdownStart = "s9",
    CreateCustomLobbyResponse = "s10",
    CreateFreeGangHousesForLevel = "s88",

    Death = "s11",

    //SyncPlayersSameLobby = "SyncPlayersSameLobby_Server",
    DetachEntityWorkaround = "s76",

    ExplodeHead = "s86",

    FreezeEntityWorkaround = "s85",
    FreezePlayerWorkaround = "s77",
    FromBrowserEventReturn = "s82",
    GotoPositionByMapCreator = "s13",

    HitOpponent = "s14",

    JoinLobby = "s15",
    JoinSameLobby = "s16",

    LeaveSameLobby = "s18",
    LoadMapForMapCreator = "s21",
    LoadOwnMapRatings = "s23",

    LoginSuccessful = "s46",
    MapChange = "s25",
    MapClear = "s26",
    MapCreatorRequestAllObjectsForPlayer = "s27",
    MapCreatorStartNewMap = "s28",
    MapCreatorSyncAllObjects = "s29",
    MapCreatorSyncFixLastId = "s31",
    MapCreatorSyncNewObject = "s32",
    MapCreatorSyncObjectPosition = "s33",
    MapCreatorSyncObjectRemove = "s34",
    MapCreatorSyncTeamObjectsRemove = "s1",
    MapsListRequest = "s35",

    Notification = "s89",

    PlayCustomSound = "s37",
    PlayerGotBomb = "s38",
    PlayerJoinedTeam = "s41",
    PlayerLeftTeam = "s42",
    PlayerPlantedBomb = "s39",
    PlayerRespawned = "s43",
    PlayerSpawn = "s90",
    PlayerSpectateMode = "s40",
    PlayerTeamChange = "s44",
    PlayerWeaponChange = "s45",
    RemoveForceStayAtPosition = "s84",
    RequestNewMapsList = "s48",
    RoundEnd = "s51",
    RoundStart = "s50",
    SetEntityCollisionlessWorkaround = "s78",
    SetEntityInvincible = "s54",
    SetForceStayAtPosition = "s83",
    SetPlayerData = "s56",
    SetPlayerInvincible = "s57",
    SetPlayerTeamWorkaround = "s79",
    SetPlayerToSpectatePlayer = "s80",
    SpectatorReattachCam = "s59",
    StartCharCreator = "s20",
    StartRankingShowAfterRound = "s60",
    StartRegisterLogin = "s61",
    StopBombPlantDefuse = "s62",
    StopRoundStats = "s64",
    StopSpectator = "s65",
    SyncPlayerCommandsSettings = "s87",
    SyncScoreboardData = "s71",
    SyncSettings = "s70",
    SyncTeamChoiceMenuData = "s72",
    SyncTeamPlayers = "s73",

    // Special //
    ToBrowserEvent = "s81",

    ToggleTeamChoiceMenu = "s74",
}

export default ToClientEvent;
