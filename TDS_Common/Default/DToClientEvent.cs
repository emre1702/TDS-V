﻿namespace TDS_Common.Default
{
    public static class DToClientEvent
    {
        public const string AddMapToVoting = "AddMapToVoting_Server";
        public const string AmountInFightSync = "AmountInFightSync_Server";
        public const string ApplySuicideAnimation = "ApplySuicideAnimation_Server";

        public const string BombDetonated = "BombDetonated_Server";
        public const string BombNotOnHand = "BombNotOnHand_Server";
        public const string BombOnHand = "BombOnHand_Server";
        public const string BombPlanted = "BombPlanted_Server";

        public const string ClearTeamPlayers = "ClearTeamPlayers_Server";
        public const string CountdownStart = "CountdownStart_Server";
        public const string CreateCustomLobbyResponse = "CreateCustomLobbyResponse_Server";

        public const string Death = "Death_Server";

        public const string GetSupportRequestData = "GetSupportRequestData_Server";
        public const string GotoPositionByMapCreator = "GotoPositionByMapCreator_Server";

        public const string HitOpponent = "HitOpponent_Server";

        public const string JoinLobby = "JoinLobby_Server";
        public const string JoinSameLobby = "JoinSameLobby_Server";

        public const string LeaveCustomLobbyMenu = "LeaveCustomLobbyMenu_Server";
        public const string LeaveSameLobby = "LeaveSameLobby_Server";
        public const string LoadApplicationDataForAdmin = "LoadApplicationDataForAdmin_Server";
        public const string LoadMapFavourites = "LoadMapFavourites_Server";
        public const string LoadMapForMapCreator = "LoadMapForMapCreator_Server";
        public const string LoadMapNamesToLoadForMapCreator = "LoadMapNamesToLoadForMapCreator_Server";
        public const string LoadOwnMapRatings = "LoadOwnMapRatings_Server";
        public const string LoadUserpanelData = "LoadUserpanelData_Server";

        public const string MapChange = "MapChange_Server";
        public const string MapClear = "MapClear_Server";
        public const string MapCreatorRequestAllObjectsForPlayer = "MapCreatorRequestAllObjectsForPlayer_Server";
        public const string MapCreatorStartNewMap = "MapCreatorStartNewMap_Server";
        public const string MapCreatorSyncAllObjects = "MapCreatorSyncAllObjects_Server";
        public const string MapCreatorSyncData = "MapCreatorSyncData_Server";
        public const string MapCreatorSyncFixLastId = "MapCreatorSyncFixLastId_Server";
        public const string MapCreatorSyncNewObject = "MapCreatorSyncNewObject_Server";
        public const string MapCreatorSyncObjectPosition = "MapCreatorSyncObjectPosition_Server";
        public const string MapCreatorSyncObjectRemove = "MapCreatorSyncObjectRemove_Server";
        public const string MapsListRequest = "MapsListRequest_Server";
        public const string MapVotingSyncOnPlayerJoin = "MapVotingSyncOnPlayerJoin_Server";

        public const string PlayCustomSound = "PlayCustomSound_Server";
        public const string PlayerGotBomb = "PlayerGotBomb_Server";
        public const string PlayerPlantedBomb = "PlayerPlantedBomb_Server";
        public const string PlayerSpectateMode = "PlayerSpectateMode_Server";
        public const string PlayerJoinedTeam = "PlayerJoinedTeam_Server";
        public const string PlayerLeftTeam = "PlayerLeftTeam_Server";
        public const string PlayerRespawned = "PlayerRespawned_Server";
        public const string PlayerTeamChange = "PlayerTeamChange_Server";
        public const string PlayerWeaponChange = "PlayerWeaponChange_Server";

        public const string RegisterLoginSuccessful = "RegisterLoginSuccessful_Server";
        public const string RemoveCustomLobby = "RemoveCustomLobby_Server";
        public const string RequestNewMapsList = "RequestNewMapsList_Server";

        public const string RemoveSyncedPlayerDatas = "RemoveSyncedPlayerDatas_Server";
        public const string RoundStart = "RoundStart_Server";
        public const string RoundEnd = "RoundEnd_Server";

        public const string SaveMapCreatorReturn = "SaveMapCreatorReturn_Server";
        public const string SendMapCreatorReturn = "SendMapCreatorReturn_Server";
        public const string SetEntityInvincible = "SetEntityInvincible_Server";
        public const string SetMapVotes = "SetMapVotes_Server";
        public const string SetPlayerData = "SetPlayerData_Server";
        public const string SetPlayerInvincible = "SetPlayerInvincible_Server";
        public const string SetSupportRequestClosed = "SetSupportRequestClosed_Server";
        public const string SpectatorReattachCam = "SpectatorReattachCam_Server";
        public const string StartRankingShowAfterRound = "StartRankingShowAfterRound_Server";
        public const string StartRegisterLogin = "StartRegisterLogin_Server";
        public const string StopBombPlantDefuse = "StopBombPlantDefuse_Server";
        public const string StopMapVoting = "StopMapVoting_Server";
        public const string StopRoundStats = "StopRoundStats_Server";
        public const string StopSpectator = "StopSpectator_Server";
        public const string SyncAllCustomLobbies = "SyncAllCustomLobbies_Server";
        public const string SyncNewCustomLobby = "SyncNewCustomLobby_Server";
        public const string SyncNewSupportRequestMessage = "SyncNewSupportRequestMessage_Server";
        public const string SyncPlayerData = "SyncPlayerData_Server";
        public const string SyncSettings = "SyncSettings_Server";
        public const string SyncScoreboardData = "SyncScoreboardData_Server";
        public const string SyncTeamChoiceMenuData = "SyncTeamChoiceMenuData_Server";
        public const string SyncTeamPlayers = "SyncTeamPlayers_Server";

        public const string ToggleTeamChoiceMenu = "ToggleTeamChoiceMenu_Server";

        //public const string SyncPlayersSameLobby = "SyncPlayersSameLobby_Server";

        // Workarounds //
        public const string AttachEntityToEntityWorkaround = "AttachEntityToEntityWorkaround_Server";

        public const string DetachEntityWorkaround = "DetachEntityWorkaround_Server";
        public const string FreezePlayerWorkaround = "FreezePlayerWorkaround_Server";
        public const string SetEntityCollisionlessWorkaround = "SetEntityCollisionlessWorkaround_Server";
        public const string SetPlayerTeamWorkaround = "SetPlayerTeamWorkaround_Server";
        public const string SetPlayerToSpectatePlayer = "SpectateWorkaround_Server";


        // Special //
        public const string ToBrowserEvent = "ToBrowserEvent_Server";
        public const string FromBrowserEventReturn = "FromBrowserEventReturn_Server";
    }
}