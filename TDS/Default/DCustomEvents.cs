namespace TDS.Default
{
    class DCustomEvents
    {
        // Go through all events, add clientside that default/enum and check parameters

        public const string ClientBombDetonated = "ClientBombDetonated";
        public const string ClientCommandUse = "ClientCommandUse";
        public const string ClientLoadOwnMapRatings = "ClientLoadOwnMapRatings";
        public const string ClientMapChange = "ClientMapChange";
        public const string ClientMoneyChange = "ClientMoneyChange";
        public const string ClientPlayerDeath = "ClientPlayerDeath";
        public const string ClientPlayerHitOpponent = "ClientPlayerHitOpponent";
        public const string ClientPlayerJoinMapCreatorLobby = "ClientPlayerJoinMapCreatorLobby";
        public const string ClientPlayerJoinLobby = "ClientPlayerJoinLobby";
        public const string ClientPlayerJoinSameLobby = "ClientPlayerJoinSameLobby";
        public const string ClientPlayerLeaveSameLobby = "ClientPlayerLeaveSameLobby";
        public const string ClientPlayerTeamChange = "ClientPlayerTeamChange";
        public const string ClientPlayerWeaponChange = "ClientPlayerWeaponChange";

        public const string GotoPositionByMapCreator = "GotoPositionByMapCreator";

        public const string RegisterLoginSuccessful = "RegisterLoginSuccessful";
        public const string RequestNewMapsList = "RequestNewMapsList";

        public const string StartRegisterLogin = "StartRegisterLogin";
        public const string SyncPlayersSameLobby = "SyncPlayersSameLobby";
    }
}
