﻿using Newtonsoft.Json;

namespace TDS_Common.Dto
{
    public class EntityCollisionlessInfoDto
    {
        public int EntityValue;
        public bool Collisionless;

        [JsonIgnore]
        public string Json;

        public EntityCollisionlessInfoDto(int EntityValue, bool Collisionless)
        {
            this.EntityValue = EntityValue;
            this.Collisionless = Collisionless;

            this.Json = JsonConvert.SerializeObject(this);
        }
    }
}