using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS_Server.Data.Models.Map
{
    #nullable disable
    public class LoadMapDialogGroupDto
    {

        [JsonProperty("0")]
        public string GroupName { get; set; }

        [JsonProperty("1")]
        public IEnumerable<LoadMapDialogMapDto> Maps { get; set; }

    }
}
