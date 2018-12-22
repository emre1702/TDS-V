using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Dto
{
    public class MapDto
    {
        public MapSynedDataDto SyncedData;
        public List<List<PositionRotationDto>> TeamSpawns { get; set; }
        public List<Vector3> MapLimits = new List<Vector3>();
        public Vector3 MapCenter;
        public List<Vector3> BombPlantPlaces = new List<Vector3>();
    }
}
