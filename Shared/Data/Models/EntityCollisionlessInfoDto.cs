using Newtonsoft.Json;
using TDS_Shared.Core;

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

        public EntityCollisionlessInfoDto(int entityValue, bool collisionless)
        {
            EntityValue = entityValue;
            Collisionless = collisionless;
        }
    }
}
