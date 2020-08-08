using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharHairAndColorsDatas 
    {
        #region Public Properties
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public int Hair { get; set; }

        [JsonProperty("1")]
        public int HairColor { get; set; }

        [JsonProperty("2")]
        public int HairHighlightColor { get; set; }

        [JsonProperty("3")]
        public int EyebrowColor { get; set; }

        [JsonProperty("4")]
        public int FacialHairColor { get; set; }

        [JsonProperty("5")]
        public int EyeColor { get; set; }

        [JsonProperty("6")]
        public int BlushColor { get; set; }

        [JsonProperty("7")]
        public int LipstickColor { get; set; }

        [JsonProperty("8")]
        public int ChestHairColor { get; set; }

        [JsonIgnore]
        public virtual PlayerCharDatas CharDatas { get; set; }

        #endregion Public Properties
    }
}
