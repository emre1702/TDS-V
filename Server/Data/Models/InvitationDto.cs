using Newtonsoft.Json;

namespace TDS_Server.Data.Models
{
    #nullable enable
    public class InvitationDto
    {
        [JsonProperty("0")]
        public ulong Id { get; set; }

        [JsonProperty("1")]
        public string? Sender { get; set; }

        [JsonProperty("2")]
        public string Message { get; set; } = string.Empty;
    }
}
