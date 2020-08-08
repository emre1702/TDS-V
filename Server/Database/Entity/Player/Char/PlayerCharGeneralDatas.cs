using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharGeneralDatas
    {
        #region Public Properties
        [JsonIgnore]
        public int PlayerId { get; set; }
        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public bool IsMale { get; set; }

        [JsonIgnore]
        public virtual PlayerCharDatas CharDatas { get; set; }

        #endregion Public Properties
    }
}
