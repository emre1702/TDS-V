using Newtonsoft.Json;

namespace TDS.Server.Data.Models.Map
{
    public class LoadMapDialogMapDto
    {

        [JsonProperty("0")]
        public int Id { get; set; }

        [JsonProperty("1")]
        public string Name { get; set; } = string.Empty;

    }
}
