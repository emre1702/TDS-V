using Newtonsoft.Json;

namespace TDS_Server.Dto
{
    public class AngularAnnouncementData
    {
        [JsonProperty("0")]
        public int DaysAgo { get; set; }

        [JsonProperty("1")]
        public string Text { get; set; } = string.Empty;
    }
}
