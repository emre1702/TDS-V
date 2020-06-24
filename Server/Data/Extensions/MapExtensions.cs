using System;
using System.Linq;
using TDS_Server.Data.Models.Map;
using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Extensions
{
#nullable enable

    public static class MapExtensions
    {
        #region Public Methods

        public static void CreateJsons(this MapDto map, Serializer serializer)
        {
            if (map.BombInfo != null)
                map.BombInfo.PlantPositionsJson = serializer.ToBrowser(map.BombInfo.PlantPositions);
            map.LimitInfo.EdgesJson = serializer.ToBrowser(map.LimitInfo.Edges);
            map.LoadMapObjectsDataDto();
        }

        public static Position3DDto? GetCenter(this MapDto map)
        {
            if (map.Info.Type == Enums.MapType.Gangwar)
                return map.Target;

            if (map.LimitInfo.Center is { } && (map.LimitInfo.Center.X != 0 || map.LimitInfo.Center.Y != 0 || map.LimitInfo.Center.Z != 0))
                return map.LimitInfo.Center;

            if (map.LimitInfo.Edges is null || map.LimitInfo.Edges.Length == 0)
                return map.GetCenterBySpawns();

            float zpos = map.GetCenterZPos();
            return map.GetCenterByLimits(zpos);
        }

        public static void LoadSyncedData(this MapDto map)
        {
            map.BrowserSyncedData.Name = map.Info.Name;
            map.BrowserSyncedData.Description[(int)Language.English] = map.Descriptions?.English;
            map.BrowserSyncedData.Description[(int)Language.German] = map.Descriptions?.German;
            map.BrowserSyncedData.Type = (MapType)(int)map.Info.Type;
        }

        #endregion Public Methods

        #region Private Methods

        private static Position3DDto? GetCenterByLimits(this MapDto map, float zpos)
        {
            return GetCenterOfMapPositions(map.LimitInfo.Edges, zpos) ?? GetCenterBySpawns(map);
        }

        private static Position3DDto? GetCenterBySpawns(this MapDto map)
        {
            int amountteams = map.TeamSpawnsList.TeamSpawns.Length;
            if (amountteams == 1)
            {
                return map.TeamSpawnsList.TeamSpawns[0].Spawns.First().To3DDto();
            }
            else if (amountteams > 1)
            {
                Position3DDto[] positions = map.TeamSpawnsList.TeamSpawns
                    .Select(entry => entry.Spawns[0].To3DDto())
                    .ToArray();
                return GetCenterOfMapPositions(positions);
            }
            return null;
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

        private static float GetCenterZPos(this MapDto map)
        {
            var teamSpawns1 = map.TeamSpawnsList.TeamSpawns.FirstOrDefault();
            if (teamSpawns1 is null || teamSpawns1.Spawns.Length == 0)
                return 0;
            var teamSpawns2 = map.TeamSpawnsList.TeamSpawns.Last();
            var spawn1 = teamSpawns1.Spawns.FirstOrDefault();
            var spawn2 = teamSpawns2.Spawns.FirstOrDefault();
            if (spawn1 is null && spawn2 is { })
                return spawn2.Z;
            if (spawn2 is null && spawn1 is { })
                return spawn1.Z;
            if (spawn1 is null && spawn2 is null)
                return 0;
            return (spawn1!.Z + spawn2!.Z) / 2;
        }

        #endregion Private Methods
    }
}
