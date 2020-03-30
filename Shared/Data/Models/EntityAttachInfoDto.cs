using Newtonsoft.Json;
using TDS_Shared.Core;

namespace TDS_Shared.Data.Models
{
    public class EntityAttachInfoDto
    {
        [JsonProperty("0")]
        public int EntityValue;
        [JsonProperty("1")]
        public int TargetValue;
        [JsonProperty("2")]
        public int Bone;
        [JsonProperty("3")]
        public float PositionOffsetX;
        [JsonProperty("4")]
        public float PositionOffsetY;
        [JsonProperty("5")]
        public float PositionOffsetZ;
        [JsonProperty("6")]
        public float RotationOffsetX;
        [JsonProperty("7")]
        public float RotationOffsetY;
        [JsonProperty("8")]
        public float RotationOffsetZ;

        [JsonIgnore]
        public int? LobbyId;

        [JsonIgnore]
        public string Json;

        public EntityAttachInfoDto(int EntityValue, int TargetValue, int Bone,
            float PositionOffsetX, float PositionOffsetY, float PositionOffsetZ,
            float RotationOffsetX, float RotationOffsetY, float RotationOffsetZ,
            int? LobbyId, Serializer serializer)
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

            this.Json = serializer.ToClient(this);
        }
    }
}
