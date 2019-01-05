﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Common.Default
{
    public class DToClientEvent
    {
        public const string AddVoteToMap = "AddVoteToMap_Server";
        public const string AmountInFightSync = "AmountInFightSync_Server";

        public const string BombDetonated = "BombDetonated_Server";
        public const string BombPlanted = "BombPlanted_Server";

        public const string CountdownStart = "CountdownStart_Server";

        public const string Death = "Death_Server";

        public const string GotoPositionByMapCreator = "GotoPositionByMapCreator_Server";

        public const string HitOpponent = "HitOpponent_Server";

        public const string JoinLobby = "JoinLobby_Server";
        public const string JoinMapCreatorLobby = "JoinMapCreatorLobby_Server";
        public const string JoinSameLobby = "JoinSameLobby_Server";

        public const string LeaveSameLobby = "LeaveSameLobby_Server";
        public const string LoadOwnMapRatings = "LoadOwnMapRatings_Server";

        public const string MapChange = "MapChange_Server";
        public const string MapsListRequest = "MapsListRequest_Server";
        public const string MapVotingSyncOnPlayerJoin = "MapVotingSyncOnPlayerJoin_Server";

        public const string PlayerAdminLevelChange = "PlayerAdminLevelChange_Server";
        public const string PlayerGotBomb = "PlayerGotBomb_Server";
        public const string PlayerMoneyChange = "PlayerMoneyChange_Server";
        public const string PlayerPlantedBomb = "PlayerPlantedBomb_Server";
        public const string PlayerSpectateMode = "PlayerSpectateMode_Server";
        public const string PlayerTeamChange = "PlayerTeamChange_Server";
        public const string PlayerWeaponChange = "PlayerWeaponChange_Server";

        public const string RegisterLoginSuccessful = "RegisterLoginSuccessful_Server";
        public const string RequestNewMapsList = "RequestNewMapsList_Server";

        public const string RoundStart = "RoundStart_Server";
        public const string RoundEnd = "RoundEnd_Server";

        public const string StartRegisterLogin = "StartRegisterLogin_Server";
        public const string SyncCurrentMapName = "SyncCurrentMapName_Server";
        public const string SyncScoreboardData = "SyncScoreboardData_Server";

        //public const string SyncPlayersSameLobby = "SyncPlayersSameLobby_Server";
    }
}