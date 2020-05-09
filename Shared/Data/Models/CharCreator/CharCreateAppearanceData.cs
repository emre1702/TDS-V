using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.CharCreator
{
    public class CharCreateAppearanceData
    {
        [JsonProperty("0")]
        public int Blemishes { get; set; }
        [JsonProperty("1")]
        public float BlemishesOpacity { get; set; }

        [JsonProperty("2")]
        public int FacialHair { get; set; }
        [JsonProperty("3")]
        public float FacialHairOpacity { get; set; }

        [JsonProperty("4")]
        public int Eyebrows { get; set; }
        [JsonProperty("5")]
        public float EyebrowsOpacity { get; set; }

        [JsonProperty("6")]
        public int Ageing { get; set; }
        [JsonProperty("7")]
        public float AgeingOpacity { get; set; }

        [JsonProperty("8")]
        public int Makeup { get; set; }
        [JsonProperty("9")]
        public float MakeupOpacity { get; set; }

        [JsonProperty("10")]
        public int Blush { get; set; }
        [JsonProperty("11")]
        public float BlushOpacity { get; set; }

        [JsonProperty("12")]
        public int Complexion { get; set; }
        [JsonProperty("13")]
        public float ComplexionOpacity { get; set; }

        [JsonProperty("14")]
        public int SunDamage { get; set; }
        [JsonProperty("15")]
        public float SunDamageOpacity { get; set; }

        [JsonProperty("16")]
        public int Lipstick { get; set; }
        [JsonProperty("17")]
        public float LipstickOpacity { get; set; }

        [JsonProperty("18")]
        public int MolesAndFreckles { get; set; }
        [JsonProperty("19")]
        public float MolesAndFrecklesOpacity { get; set; }

        [JsonProperty("20")]
        public int ChestHair { get; set; }
        [JsonProperty("21")]
        public float ChestHairOpacity { get; set; }

        [JsonProperty("22")]
        public int BodyBlemishes { get; set; }
        [JsonProperty("23")]
        public float BodyBlemishesOpacity { get; set; }

        [JsonProperty("24")]
        public int AddBodyBlemishes { get; set; }
        [JsonProperty("25")]
        public float AddBodyBlemishesOpacity { get; set; }
    }
}
