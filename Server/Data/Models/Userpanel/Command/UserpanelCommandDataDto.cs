﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS.Server.Data.Models.Userpanel.Command
{
#nullable disable

    public class UserpanelCommandDataDto
    {
        #region Public Properties

        [JsonProperty("6")]
        public List<string> Aliases { get; set; }

        [JsonProperty("0")]
        public string Command { get; set; }

        [JsonProperty("7")]
        public Dictionary<int, string> Description { get; set; }

        // Used for UserpanelPlayerCommandsHandler
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("4")]
        public bool LobbyOwnerCanUse { get; set; }

        [JsonProperty("1")]
        public short? MinAdminLevel { get; set; }

        [JsonProperty("2")]
        public short? MinDonation { get; set; }

        [JsonProperty("5")]
        public List<UserpanelCommandSyntaxDto> Syntaxes { get; set; } = new List<UserpanelCommandSyntaxDto>();

        [JsonProperty("3")]
        public bool VIPCanUse { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
