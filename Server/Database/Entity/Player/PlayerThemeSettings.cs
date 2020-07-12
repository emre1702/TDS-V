using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Shared.Data.Models;

namespace TDS_Server.Database.Entity.Player
{
    public class PlayerThemeSettings : SyncedPlayerThemeSettings
    {
        #region Public Properties

        [JsonIgnore]
        public virtual Players Player { get; set; }

        [JsonIgnore]
        public int PlayerId { get; set; }

        #endregion Public Properties
    }
}
