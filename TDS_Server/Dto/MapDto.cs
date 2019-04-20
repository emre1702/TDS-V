using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Dto;

namespace TDS_Server.Dto
{
    public class MapDto
    {
        public uint CreatorID;
        public SyncedMapDataDto SyncedData => new SyncedMapDataDto();
        public List<List<PositionRotationDto>> TeamSpawns { get; set; } = new List<List<PositionRotationDto>>();
        public List<Vector3> MapLimits = new List<Vector3>();
        public Vector3 MapCenter = new Vector3();
        public List<Vector3> BombPlantPlaces = new List<Vector3>();

    }
}
