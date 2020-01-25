using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS_Server.Dto
{
    public class LoadMapDialogGroupDto
    {
        [JsonProperty("0")]
        public string GroupName { get; set; } = string.Empty;
        [JsonProperty("1")]
        public List<string> Maps { get; set; } = new List<string>();
    }
}
