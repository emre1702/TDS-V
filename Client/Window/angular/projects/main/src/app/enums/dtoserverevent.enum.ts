export enum DToServerEvent {
    AcceptInvitation = "c1",
    AcceptTDSTeamInvitation = "c67",
    AnswerToOfflineMessage = "c3",
    BuyMap = "c4",
    CancelCharCreateData = "c69",
    ChooseTeam = "c6",
    CreateCustomLobby = "c8",
    CreateGang = "c74",

    DeleteOfflineMessage = "c9",
    GangCommand = "c76",
    GetSupportRequestData = "c10",
    GetVehicle = "c11",
    JoinedCustomLobbiesMenu = "c14",
    JoinLobby = "c15",
    JoinLobbyWithPassword = "c16",
    LeaveLobby = "c19",
    LeftCustomLobbiesMenu = "c20",
    LeftSupportRequest = "c21",
    LeftSupportRequestsList = "c22",
    LoadAllMapsForCustomLobby = "c65",
    LoadApplicationDataForAdmin = "c24",
    LoadDatasForCustomLobby = "c66",
    LoadGangWindowData = "c75",
    LoadMapNamesToLoadForMapCreator = "c26",
    LoadMapForMapCreator = "c25",
    LoadPlayerWeaponStats = "c72",
    LoadUserpanelData = "c27",
    MapCreatorSyncCurrentMapToServer = "c17",
    MapCreatorSyncData = "c30",
    MapVote = "c36",
    RejectInvitation = "c38",
    RejectTDSTeamInvitation = "c68",
    RemoveMap = "c39",
    SaveCharCreateData = "c29",
    SaveMapCreatorData = "c41",
    SavePlayerCommandsSettings = "c70",
    SaveSettings = "c42",
    SaveSpecialSettingsChange = "c43",
    SendApplication = "c44",
    SendApplicationInvite = "c45",
    SendMapCreatorData = "c46",
    SendOfflineMessage = "c48",
    SendSupportRequest = "c49",
    SendSupportRequestMessage = "c50",
    SetDamageTestWeaponDamage = "c79",
    SetSupportRequestClosed = "c52",
    ToggleMapFavouriteState = "c61",

    FromBrowserEvent = "c64"
}
