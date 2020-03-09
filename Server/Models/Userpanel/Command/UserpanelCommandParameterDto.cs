using Newtonsoft.Json;

namespace TDS_Server.Dto.Userpanel.Command
{
    #nullable disable
    public class UserpanelCommandParameterDto
    {
        [JsonProperty("0")]
        public string Name { get; set; }
        [JsonProperty("1")]
        public string Type { get; set; }
        [JsonProperty("2")]
        public object DefaultValue { get; set; }
    }
    #nullable restore
}
