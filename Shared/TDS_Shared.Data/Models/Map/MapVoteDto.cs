using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.Map
{
    public class MapVoteDto
    {
        [JsonProperty("0")]
        public int Id { get; set; }
        [JsonProperty("1")]
        public string Name { get; set; }
        [JsonProperty("2")]
        public int AmountVotes { get; set; }

    }
}
