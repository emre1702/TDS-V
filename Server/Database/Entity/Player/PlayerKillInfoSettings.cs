using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Database.Entity.Player
{
    public class PlayerKillInfoSettings
    {
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("2000")]
        public bool ShowIcon { get; set; }

        [JsonProperty("2001")]
        public float FontWidth { get; set; }

        [JsonProperty("2002")]
        public int IconWidth { get; set; }

        [JsonProperty("2003")]
        public int Spacing { get; set; }

        [JsonIgnore]
        public virtual Players Player { get; set; }

    }
}
