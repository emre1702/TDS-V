using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS_Shared.Data.Models.PlayerCommands
{
    public class UserpanelPlayerCommandData
    {
        #region Public Properties

        [JsonProperty("1")]
        public List<UserpanelPlayerConfiguredCommandData> AddedCommands { get; set; }

        [JsonProperty("0")]
        public List<UserpanelPlayerCommandCommandData> InitialCommands { get; set; }

        #endregion Public Properties
    }
}
