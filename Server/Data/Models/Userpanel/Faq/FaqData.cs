using Newtonsoft.Json;

namespace TDS.Server.Data.Models.Userpanel.Faq
{
    public class FaqData
    {
        [JsonProperty("2")]
        public string Answer { get; set; }

        [JsonProperty("0")]
        public int Id { get; set; }

        [JsonProperty("1")]
        public string Question { get; set; }
    }
}
