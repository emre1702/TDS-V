using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Common.Dto
{
    public class EntityCollisionlessInfoDto
    {
        public int EntityValue;
        public bool Collisionless;
        [JsonIgnore]
        public string Json;
    }
}
