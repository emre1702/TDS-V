﻿using GTANetworkAPI;

namespace TDS.Server.Data.Defaults
{
    public static class Constants
    {
        public const string MapsPath = _mapFoldersPath + "maps/";
        public const string NeedCheckMapsPath = _mapFoldersPath + "needcheckmaps/";
        public const string NewMapsPath = _mapFoldersPath + "newmaps/";
        public const string SavedMapsPath = _mapFoldersPath + "savedmaps/";
        public const string ErrorFilePath = _volume + "errorfiles/";
        public const float ArenaHeadMultiplicator = 1.7f;
        public const string GangwarTargetObjectName = "v_ret_ta_skull";
        public const int RemoveTDSPlayerMinutesAfterLoggedOut = 5;

        public static readonly (Vector3, float)[] RoundRankingPositions = new (Vector3, float)[3]
        {
            (new Vector3(-425.48, 1123.55, 325.85), 345),
            (new Vector3(-427.03, 1123.21, 325.85), 345),
            (new Vector3(-424.33, 1122.5, 325.85), 345)
        };

        public static readonly Vector3 RoundRankingSpectatorPosition = new Vector3(-425.48, 1123.55, 335.85);

        private const string _volume = "/ragemp-server-data/";
        private const string _mapFoldersPath = _volume + "Maps/";
    }
}