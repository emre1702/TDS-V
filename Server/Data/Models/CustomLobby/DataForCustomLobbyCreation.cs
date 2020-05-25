using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS_Server.Data.Models.CustomLobby
{
    public class DataForCustomLobbyCreation
    {
#nullable disable

        #region Public Properties

        [JsonProperty("1")]
        public List<CustomLobbyWeaponData> ArenaWeaponDatas { get; set; }

        [JsonProperty("0")]
        public List<CustomLobbyWeaponData> WeaponDatas { get; set; }

        #endregion Public Properties
    }
}
