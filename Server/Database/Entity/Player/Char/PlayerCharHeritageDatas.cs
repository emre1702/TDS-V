using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharHeritageDatas
    {
        #region Public Properties
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public int FatherIndex { get; set; }

        [JsonProperty("1")]
        public int MotherIndex { get; set; }

        [JsonProperty("2")]
        public float ResemblancePercentage { get; set; }

        [JsonProperty("3")]
        public float SkinTonePercentage { get; set; }

        [JsonIgnore]
        public virtual PlayerCharDatas CharDatas { get; set; }

        #endregion Public Properties
    }
}
