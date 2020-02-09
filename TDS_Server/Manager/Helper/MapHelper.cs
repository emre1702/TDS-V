using GTANetworkAPI;
using System;
using System.Linq;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Dto.Map;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Helper
{
    internal static class MapHelper
    {
        public static void LoadSyncedData(this MapDto map)
        {
            map.BrowserSyncedData.Name = map.Info.Name;
            map.BrowserSyncedData.Description[(int)ELanguage.English] = map.Descriptions?.English;
            map.BrowserSyncedData.Description[(int)ELanguage.German] = map.Descriptions?.German;
            map.BrowserSyncedData.Type = (EMapType) (int)map.Info.Type;
        }

        public static void LoadMapRatings(this MapDto map, TDSDbContext dbContext)
        {
            map.Ratings = dbContext.PlayerMapRatings.Where(m => m.MapId == map.BrowserSyncedData.Id).ToList();
            map.RatingAverage = map.Ratings.Count > 0 ? map.Ratings.Average(r => r.Rating) : 5;
        }

        public static void CreateJsons(this MapDto map)
        {
            if (map.BombInfo != null)
                map.BombInfo.PlantPositionsJson = Serializer.ToBrowser(map.BombInfo.PlantPositions);
            map.LimitInfo.EdgesJson = Serializer.ToBrowser(map.LimitInfo.Edges);
            map.LoadMapObjectsDataDto();
        }

        public static Position3DDto? GetCenter(this MapDto map)
        {
            if (map.LimitInfo.Edges is null || map.LimitInfo.Edges.Length == 0)
                return map.GetCenterBySpawns();

            float zpos = map.GetCenterZPos();
            return map.GetCenterByLimits(zpos);
        }

        private static float GetCenterZPos(this MapDto map)
        {
            var teamSpawns1 = map.TeamSpawnsList.TeamSpawns.FirstOrDefault();
            var teamSpawns2 = map.TeamSpawnsList.TeamSpawns.LastOrDefault();
            if (teamSpawns1 is null)
                return 0;
            var spawn1 = teamSpawns1.Spawns.FirstOrDefault();
            var spawn2 = teamSpawns2.Spawns.FirstOrDefault();
            return (spawn1.Z + spawn2.Z) / 2;
        }

        private static Position3DDto? GetCenterByLimits(this MapDto map, float zpos)
        {
            return GetCenterOfMapPositions(map.LimitInfo.Edges, zpos) ?? GetCenterBySpawns(map);
        }

        private static Position3DDto? GetCenterOfMapPositions(Position3DDto[]? positions, float zpos = 0)
        {
            if (positions == null || positions.Length <= 2)
                return null;

            float centerX = 0.0f;
            float centerY = 0.0f;
            float centerZ = 0.0f;

            foreach (Position3DDto point in positions)
            {
                centerX += point.X;
                centerY += point.Y;
                centerZ += Math.Abs(zpos - (-1)) < 0.001 ? point.Z : zpos;
            }

            return new Position3DDto { X = centerX / positions.Length, Y = centerY / positions.Length, Z = centerZ / positions.Length };
        }

        private static Position3DDto? GetCenterBySpawns(this MapDto map)
        {
            int amountteams = map.TeamSpawnsList.TeamSpawns.Length;
            if (amountteams == 1)
            {
                return map.TeamSpawnsList.TeamSpawns[0].Spawns.FirstOrDefault().To3D();
            }
            else if (amountteams > 1)
            {
                Position3DDto[] positions = map.TeamSpawnsList.TeamSpawns
                    .Select(entry => entry.Spawns[0].To3D())
                    .ToArray();
                return GetCenterOfMapPositions(positions);
            }
            return null;
        }

        public static Vector3 ToVector3(this Position4DDto pos)
        {
            return new Vector3(pos.X, pos.Y, pos.Z);
        }

        public static TDS_Common.Dto.Map.Position3DDto SwitchNamespace(this Position3DDto dto) 
        {
            return new TDS_Common.Dto.Map.Position3DDto { X = dto.X, Y = dto.Y, Z = dto.Z };
        }

        public static Position3DDto SwitchNamespace(this TDS_Common.Dto.Map.Position3DDto dto)
        {
            return new Position3DDto { X = dto.X, Y = dto.Y, Z = dto.Z };
        }
    }
}
