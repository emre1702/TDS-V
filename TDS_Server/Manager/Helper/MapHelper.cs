﻿using Newtonsoft.Json;
using System;
using System.Linq;
using TDS_Common.Dto.Map;
using TDS_Common.Enum;

namespace TDS_Server.Manager.Helper
{
    static class MapHelper
    {
        public static void LoadSyncedData(this MapDto map)
        {
            map.SyncedData.Name = map.Info.Name;
            map.SyncedData.Description[ELanguage.English] = map.Descriptions?.English;
            map.SyncedData.Description[ELanguage.German] = map.Descriptions?.German;
            map.SyncedData.Type = map.Info.Type;
        }

        public static void CreateJsons(this MapDto map)
        {
            if (map.BombInfo != null)
                map.BombInfo.PlantPositionsJson = JsonConvert.SerializeObject(map.BombInfo.PlantPositions);
            map.LimitInfo.EdgesJson = JsonConvert.SerializeObject(map.LimitInfo.Edges);
        }

        public static MapPositionDto? GetCenter(this MapDto map)
        {
            if (map.LimitInfo.Edges.Length == 0)
                return map.GetCenterBySpawns();

            float zpos = map.GetCenterZPos();
            return map.GetCenterByLimits(zpos);

        }

        private static float GetCenterZPos(this MapDto map)
        {
            var teamSpawns1 = map.TeamSpawnsList.TeamSpawns.FirstOrDefault();
            var teamSpawns2 = map.TeamSpawnsList.TeamSpawns.LastOrDefault();
            if (teamSpawns1 == null)
                return 0;
            var spawn1 = teamSpawns1.Spawns.FirstOrDefault();
            var spawn2 = teamSpawns2.Spawns.FirstOrDefault();
            return ((spawn1.Z ?? 0) + (spawn2.Z ?? 0)) / 2;
        }

        private static MapPositionDto? GetCenterByLimits(this MapDto map, float zpos)
        {
            return GetCenterOfMapPositions(map.LimitInfo.Edges, zpos) ?? GetCenterBySpawns(map);
        }

        private static MapPositionDto? GetCenterOfMapPositions(MapPositionDto[] positions, float zpos = 0)
        {
            if (positions.Length <= 2)
                return null;

            float centerX = 0.0f;
            float centerY = 0.0f;
            float centerZ = 0.0f;


            foreach (MapPositionDto point in positions)
            {
                centerX += point.X;
                centerY += point.Y;
                centerZ += Math.Abs(zpos - (-1)) < 0.001 ? (point.Z ?? 0) : zpos;
            }

            return new MapPositionDto { X = centerX / positions.Length, Y = centerY / positions.Length, Z = centerZ / positions.Length };
        }

        private static MapPositionDto? GetCenterBySpawns(this MapDto map)
        {
            int amountteams = map.TeamSpawnsList.TeamSpawns.Length;
            if (amountteams == 1)
            {
                return map.TeamSpawnsList.TeamSpawns[0].Spawns.FirstOrDefault();
            }
            else if (amountteams > 1)
            {
                MapPositionDto[] positions = map.TeamSpawnsList.TeamSpawns
                    .Select(entry => entry.Spawns[0])
                    .ToArray();
                return GetCenterOfMapPositions(positions);
            }
            return null;
        }
    }
}
