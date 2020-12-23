export enum DToClientEvent {
    LoadMapForMapCreator = 'b1',
    CharCreatorDataChanged = 'b10',
    ChatUsed = 'b3',
    CloseChat = 'b6',
    CloseGangWindow = 'b42',
    CloseMapVotingMenu = 'b7',
    CloseUserpanel = 'b8',
    CommandUsed = 'b9',
    GetHashedPassword = 'b11',
    HoldMapCreatorObject = 'b13',
    InputStarted = 'b14',
    InputStopped = 'b15',
    LanguageChange = 'b19',
    // RemoveMapVote = "RemoveMapVote_Browser",
    MapCreatorHighlightPos = 'b21',
    MapCreatorStartNew = 'b24',
    MapCreatorStartObjectChoice = 'b25',
    MapCreatorStopObjectPreview = 'b26',
    MapCreatorStartVehicleChoice = 'b27',
    MapCreatorStopVehiclePreview = 'b28',
    OnColorSettingChange = 'b29',
    RemoveMapCreatorPosition = 'b30',
    RemoveMapCreatorTeamNumber = 'b31',
    ResetPassword = 'b43',
    TeleportToPositionRotation = 'b38',
    TryLogin = 'b40',
    TryRegister = 'b41',
    StartMapCreatorPosPlacing = 'b35',
}
