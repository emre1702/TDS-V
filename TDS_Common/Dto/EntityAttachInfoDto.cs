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

        public EntityAttachInfoDto(int EntityValue, int TargetValue, int Bone,
            float PositionOffsetX, float PositionOffsetY, float PositionOffsetZ,
            float RotationOffsetX, float RotationOffsetY, float RotationOffsetZ,
            uint? LobbyId)
        {
            this.EntityValue = EntityValue;
            this.TargetValue = TargetValue;
            this.Bone = Bone;
            this.PositionOffsetX = PositionOffsetX;
            this.PositionOffsetY = PositionOffsetY;
            this.PositionOffsetZ = PositionOffsetZ;
            this.RotationOffsetX = RotationOffsetX;
            this.RotationOffsetY = RotationOffsetY;
            this.RotationOffsetZ = RotationOffsetZ;
            this.LobbyId = LobbyId;

            this.Json = JsonConvert.SerializeObject(this);
        }
    }
}