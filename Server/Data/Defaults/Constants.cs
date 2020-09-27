﻿using GTANetworkAPI;

namespace TDS_Server.Data.Defaults
{
    public static class Constants
    {
        public const string MapsPath = _resourcePathPath + "maps/";
        public const string NeedCheckMapsPath = _resourcePathPath + "needcheckmaps/";
        public const string NewMapsPath = _resourcePathPath + "newmaps/";
        public const string SavedMapsPath = _resourcePathPath + "savedmaps/";
        public static float ArenaHeadMultiplicator = 1.7f;
        public static string GangwarTargetObjectName = "v_ret_ta_skull";
        public static int RemoveTDSPlayerMinutesAfterLoggedOut = 5;

        public static (Vector3, float)[] RoundRankingPositions = new (Vector3, float)[3]
        {
            (new Vector3(-425.48, 1123.55, 325.85), 345),
            (new Vector3(-427.03, 1123.21, 325.85), 345),
            (new Vector3(-424.33, 1122.5, 325.85), 345)
        };

        public static Vector3 RoundRankingSpectatorPosition = new Vector3(-425.48, 1123.55, 335.85);

        private const string _resourcePathPath = "dotnet/resources/tds/";
    }
}
