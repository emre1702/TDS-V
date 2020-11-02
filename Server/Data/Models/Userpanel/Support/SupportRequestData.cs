using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Data.Models.Userpanel.Support
{
    public class SupportRequestData
    {
        [JsonProperty("4")]
        public int AtleastAdminLevel { get; set; }

        [JsonIgnore]
        public int AuthorId { get; set; }

        [JsonProperty("5")]
        public bool Closed { get; set; }

        [JsonIgnore]
        public DateTime CreateTimeDate { get; set; }

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("2")]
        public IEnumerable<SupportRequestMessageData> Messages { get; set; } = new List<SupportRequestMessageData>();

        [JsonProperty("1")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("3")]
        public SupportType Type { get; set; }
    }
}
