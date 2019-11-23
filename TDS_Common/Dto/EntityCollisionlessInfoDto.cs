using MessagePack;
using TDS_Common.Manager.Utility;

namespace TDS_Common.Dto
{

    [MessagePackObject]
    public class EntityCollisionlessInfoDto
    {
        [Key(0)]
        public int EntityValue;
        [Key(1)]
        public bool Collisionless;

        [IgnoreMember]
        public string Json;

        public EntityCollisionlessInfoDto(int EntityValue, bool Collisionless)
        {
            this.EntityValue = EntityValue;
            this.Collisionless = Collisionless;

            this.Json = Serializer.ToClient(this);
        }
    }
}