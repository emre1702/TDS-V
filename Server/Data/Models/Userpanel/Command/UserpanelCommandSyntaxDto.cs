using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS_Server.Data.Models.Userpanel.Command
{
    public class UserpanelCommandSyntaxDto
    {
        [JsonProperty("0")]
        public List<UserpanelCommandParameterDto> Parameters { get; set; } = new List<UserpanelCommandParameterDto>();
    }
}
