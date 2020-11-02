using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS_Server.Data.Models.Map
{
    public class LoadMapDialogGroupDto
    {

        [JsonProperty("0")]
        public string GroupName { get; set; } = string.Empty;

        [JsonProperty("1")]
        public List<LoadMapDialogMapDto> Maps { get; set; } = new List<LoadMapDialogMapDto>();

    }
}
