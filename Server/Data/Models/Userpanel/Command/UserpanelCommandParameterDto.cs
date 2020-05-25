using Newtonsoft.Json;

namespace TDS_Server.Data.Models.Userpanel.Command
{
#nullable disable

    public class UserpanelCommandParameterDto
    {
        #region Public Properties

        [JsonProperty("2")]
        public object DefaultValue { get; set; }

        [JsonProperty("0")]
        public string Name { get; set; }

        [JsonProperty("1")]
        public string Type { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
