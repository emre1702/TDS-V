using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TDS.Server.Data.Models.Changelogs
{
    public class ChangelogsGroup
    {
        [JsonProperty("0")]
        public DateTime Date { get; set; }

        [JsonProperty("1")]
        public IEnumerable<string> Changes { get; set; }
    }
}
