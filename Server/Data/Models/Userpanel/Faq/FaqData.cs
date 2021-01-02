using Newtonsoft.Json;

namespace TDS.Server.Data.Models.Userpanel.Faq
{
    public class FaqData
    {
        [JsonProperty("1")]
        public string Answer { get; set; }

        [JsonProperty("0")]
        public string Question { get; set; }
    }
}