using Newtonsoft.Json;
using TDS_Common.Manager.Utility;

namespace TDS_Shared.Data.Models
{
    public class EntityCollisionlessInfoDto
    {
        [JsonProperty("0")]
        public int EntityValue;
        [JsonProperty("1")]
        public bool Collisionless;

        [JsonIgnore]
        public string Json;

        public EntityCollisionlessInfoDto(int EntityValue, bool Collisionless)
        {
            this.EntityValue = EntityValue;
            this.Collisionless = Collisionless;

            this.Json = Serializer.ToClient(this);
        }
    }
}
