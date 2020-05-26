using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.PlayerCommands
{
    public class UserpanelPlayerConfiguredCommandData
    {
        #region Public Properties

        [JsonProperty("0")]
        public int CommandId { get; set; }

        [JsonProperty("1")]
        public string CustomCommand { get; set; }

        #endregion Public Properties
    }
}
