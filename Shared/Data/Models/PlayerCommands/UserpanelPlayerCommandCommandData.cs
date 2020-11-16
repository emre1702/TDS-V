using Newtonsoft.Json;

namespace TDS.Shared.Data.Models.PlayerCommands
{
    public class UserpanelPlayerCommandCommandData
    {
        #region Public Properties

        [JsonProperty("1")]
        public string Command { get; set; }

        [JsonProperty("0")]
        public int Id { get; set; }

        #endregion Public Properties
    }
}
