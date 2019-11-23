using MessagePack;
using TDS_Common.Manager.Utility;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class EntityAttachInfoDto
    {
        [Key(0)]
        public int EntityValue;
        [Key(1)]
        public int TargetValue;
        [Key(2)]
        public int Bone;
        [Key(3)]
        public float PositionOffsetX;
        [Key(4)]
        public float PositionOffsetY;
        [Key(5)]
        public float PositionOffsetZ;
        [Key(6)]
        public float RotationOffsetX;
        [Key(7)]
        public float RotationOffsetY;
        [Key(8)]
        public float RotationOffsetZ;

        [IgnoreMember]
        public int? LobbyId;

        [IgnoreMember]
        public string Json;

        public EntityAttachInfoDto(int EntityValue, int TargetValue, int Bone,
            float PositionOffsetX, float PositionOffsetY, float PositionOffsetZ,
            float RotationOffsetX, float RotationOffsetY, float RotationOffsetZ,
            int? LobbyId)
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

            this.Json = Serializer.ToClient(this);
        }
    }
}