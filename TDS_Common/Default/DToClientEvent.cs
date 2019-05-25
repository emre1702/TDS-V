namespace TDS_Common.Default
{
    public class DToClientEvent
    {
        public const string SetAssistsForRoundStats = "SetAssistsForRoundStats_Server";
        public const string SetDamageForRoundStats = "SetDamageForRoundStats_Server";
        public const string SetKillsForRoundStats = "SetKillsForRoundStats_Server";

        public const string AddMapToVoting = "AddMapToVoting_Server";
        public const string AmountInFightSync = "AmountInFightSync_Server";

        public const string BombDetonated = "BombDetonated_Server";
        public const string BombNotOnHand = "BombNotOnHand_Server";
        public const string BombOnHand = "BombOnHand_Server";
        public const string BombPlanted = "BombPlanted_Server";

        public const string ClearTeamPlayers = "ClearTeamPlayers_Server";
        public const string CountdownStart = "CountdownStart_Server";

        public const string Death = "Death_Server";

        public const string GotoPositionByMapCreator = "GotoPositionByMapCreator_Server";

        public const string HitOpponent = "HitOpponent_Server";

        public const string JoinLobby = "JoinLobby_Server";
        public const string JoinMapCreatorLobby = "JoinMapCreatorLobby_Server";
        public const string JoinSameLobby = "JoinSameLobby_Server";

        public const string LeaveSameLobby = "LeaveSameLobby_Server";
        public const string LoadMapFavourites = "LoadMapFavourites_Server";
        public const string LoadOwnMapRatings = "LoadOwnMapRatings_Server";

        public const string MapChange = "MapChange_Server";
        public const string MapClear = "MapClear_Server";
        public const string MapsListRequest = "MapsListRequest_Server";
        public const string MapVotingSyncOnPlayerJoin = "MapVotingSyncOnPlayerJoin_Server";

        public const string PlayerAdminLevelChange = "PlayerAdminLevelChange_Server";
        public const string PlayerGotBomb = "PlayerGotBomb_Server";
        public const string PlayerMoneyChange = "PlayerMoneyChange_Server";
        public const string PlayerPlantedBomb = "PlayerPlantedBomb_Server";
        public const string PlayerSpectateMode = "PlayerSpectateMode_Server";
        public const string PlayerJoinedTeam = "PlayerJoinedTeam_Server";
        public const string PlayerLeftTeam = "PlayerLeftTeam_Server";
        public const string PlayerTeamChange = "PlayerTeamChange_Server";
        public const string PlayerWeaponChange = "PlayerWeaponChange_Server";

        public const string RegisterLoginSuccessful = "RegisterLoginSuccessful_Server";
        public const string RequestNewMapsList = "RequestNewMapsList_Server";

        public const string RoundStart = "RoundStart_Server";
        public const string RoundEnd = "RoundEnd_Server";

        public const string SetMapVotes = "SetMapVotes_Server";
        public const string StartRegisterLogin = "StartRegisterLogin_Server";
        public const string StopRoundStats = "StopRoundStats_Server";
        public const string SyncCurrentMapName = "SyncCurrentMapName_Server";
        public const string SyncScoreboardData = "SyncScoreboardData_Server";
        public const string SyncTeamPlayers = "SyncTeamPlayers_Server";

        //public const string SyncPlayersSameLobby = "SyncPlayersSameLobby_Server";

        // Workarounds //
        public const string AttachEntityToEntityWorkaround = "AttachEntityToEntityWorkaround_Server";

        public const string DetachEntityWorkaround = "DetachEntityWorkaround_Server";
        public const string FreezePlayerWorkaround = "FreezePlayerWorkaround_Server";
        public const string SetEntityCollisionlessWorkaround = "SetEntityCollisionlessWorkaround_Server";
        public const string SetPlayerTeamWorkaround = "SetPlayerTeamWorkaround_Server";
        public const string UnspectatePlayerWorkaround = "UnspectatePlayerWorkaround_Server";
    }
}