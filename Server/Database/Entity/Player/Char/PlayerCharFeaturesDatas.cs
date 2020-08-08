using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharFeaturesDatas
    {
        #region Public Properties
        [JsonIgnore]
        public int PlayerId { get; set; }

        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public float NoseWidth { get; set; }
        [JsonProperty("1")]
        public float NoseHeight { get; set; }
        [JsonProperty("2")]
        public float NoseLength { get; set; }
        [JsonProperty("3")]
        public float NoseBridge { get; set; }
        [JsonProperty("4")]
        public float NoseTip { get; set; }
        [JsonProperty("5")]
        public float NoseBridgeShift { get; set; }
        [JsonProperty("6")]
        public float BrowHeight { get; set; }
        [JsonProperty("7")]
        public float BrowWidth { get; set; }
        [JsonProperty("8")]
        public float CheekboneHeight { get; set; }
        [JsonProperty("9")]
        public float CheekboneWidth { get; set; }
        [JsonProperty("10")]
        public float CheeksWidth { get; set; }
        [JsonProperty("11")]
        public float Eyes { get; set; }
        [JsonProperty("12")]
        public float Lips { get; set; }
        [JsonProperty("13")]
        public float JawWidth { get; set; }
        [JsonProperty("14")]
        public float JawHeight { get; set; }
        [JsonProperty("15")]
        public float ChinLength { get; set; }
        [JsonProperty("16")]
        public float ChinPosition { get; set; }
        [JsonProperty("17")]
        public float ChinWidth { get; set; }
        [JsonProperty("18")]
        public float ChinShape { get; set; }
        [JsonProperty("19")]
        public float NeckWidth { get; set; }

        [JsonIgnore]
        public virtual PlayerCharDatas CharDatas { get; set; }
        #endregion Public Properties
    }
}
