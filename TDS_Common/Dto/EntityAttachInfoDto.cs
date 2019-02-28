using Newtonsoft.Json;

namespace TDS_Common.Dto
{
    public class EntityAttachInfoDto
    {
        public int EntityValue;
        public int TargetValue;
        public int Bone;
        public float PositionOffsetX;
        public float PositionOffsetY;
        public float PositionOffsetZ;
        public float RotationOffsetX;
        public float RotationOffsetY;
        public float RotationOffsetZ;
        [JsonIgnore]
        public uint? LobbyId;
        [JsonIgnore]
        public string Json;
    }
}
