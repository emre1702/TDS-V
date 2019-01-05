﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Common.Default
{
    public class DToServerEvent
    {
        public const string AddMapToVoting = "AddMapToVoting_Client";
        public const string AddRatingToMap = "AddRatingToMap_Client";
        public const string AddVoteToMap = "AddVoteToMap_Client";

        public const string ChatLoaded = "ChatLoaded_Client";
        public const string CommandUsed = "CommandUsed_Client";

        public const string HitOtherPlayer = "HitOtherPlayer_Client";

        public const string JoinLobby = "JoinLobby_Client";

        public const string LanguageChange = "LanguageChange_Client";

        public const string MapsListRequest = "MapsListRequest_Client";

        public const string OutsideMapLimit = "OutsideMapLimit_Client";

        public const string RequestPlayersForScoreboard = "RequestPlayersForScoreboard_Client";

        public const string StartDefusing = "StartDefusing_Client";
        public const string StartPlanting = "StartPlanting_Client";

        public const string SpectateNext = "SpectateNext_Client";
        public const string StopDefusing = "StopDefusing_Client";
        public const string StopPlanting = "StopPlanting_Client";

        public const string TryLogin = "TryLogin_Client";
        public const string TryRegister = "TryRegister_Client";
    }
}