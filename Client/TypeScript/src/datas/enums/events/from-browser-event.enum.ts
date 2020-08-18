enum FromBrowserEvent {
    CharCreatorDataChanged = "b10",
    ChatUsed = "b3",
    ChooseArenaToJoin = "b4",
    ChooseCharCreatorToJoin = "b20",
    ChooseGangLobbyToJoin = "b43",
    ChooseMapCreatorToJoin = "b5",
    CloseChat = "b6",
    CloseGangWindow = "b42",
    CloseMapVotingMenu = "b7",
    CloseUserpanel = "b8",
    CommandUsed = "b9",

    GetHashedPassword = "b11",
    GetVehicle = "b12",

    HoldMapCreatorObject = "b13",

    InputStarted = "b14",
    InputStopped = "b15",
    JoinCustomLobby = "b16",
    JoinCustomLobbyWithPassword = "b17",
    JoinedCustomLobbiesMenu = "b18",

    LanguageChange = "b19",
    LoadMapForMapCreator = "b1",

    MapCreatorHighlightPos = "b21",
    MapCreatorShowObject = "b22",
    MapCreatorShowVehicle = "b23",
    MapCreatorStartNew = "b24",
    MapCreatorStartObjectChoice = "b25",
    MapCreatorStartVehicleChoice = "b27",
    MapCreatorStopObjectPreview = "b26",
    MapCreatorStopVehiclePreview = "b28",

    OnColorSettingChange = "b29",

    ReloadPlayerSettings = "b2",
    RemoveMapCreatorPosition = "b30",
    RemoveMapCreatorTeamNumber = "b31",

    SendMapRating = "b34",
    StartMapCreatorPosPlacing = "b35",
    SyncRegisterLoginLanguageTexts = "b36",

    TeleportToPositionRotation = "b38",
    TeleportToXY = "b37",
    ToggleMapFavorite = "b39",
    TryLogin = "b40",
    TryRegister = "b41"
}

export default FromBrowserEvent;
