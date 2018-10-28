namespace TDS.Default
{
    class DCustomEvents
    {
        // Go through all events, add clientside that default/enum and check parameters
        public const string ClientLoadOwnMapRatings = "ClientLoadOwnMapRatings";
        public const string ClientMoneyChange = "ClientMoneyChange";
        public const string ClientPlayerJoinSameLobby = "ClientPlayerJoinSameLobby";
        public const string ClientPlayerLeaveSameLobby = "ClientPlayerLeaveSameLobby";
        public const string ClientPlayerTeamChange = "ClientPlayerTeamChange";

        public const string RegisterLoginSuccessful = "RegisterLoginSuccessful";
        public const string RequestNewMapsList = "RequestNewMapsList";

        public const string StartRegisterLogin = "StartRegisterLogin";
        public const string SyncPlayersSameLobby = "SyncPlayersSameLobby";
    }
}
