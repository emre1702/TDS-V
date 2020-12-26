using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS.Shared.Data.Models.CharCreator.Body
{
    public class BodyData
    {
        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public virtual List<BodyGeneralData> GeneralDataSynced { get; set; }

        [JsonProperty("1")]
        public virtual List<BodyHeritageData> HeritageDataSynced { get; set; }

        [JsonProperty("2")]
        public virtual List<BodyFeaturesData> FeaturesDataSynced { get; set; }

        [JsonProperty("3")]
        public virtual List<BodyAppearanceData> AppearanceDataSynced { get; set; }

        [JsonProperty("4")]
        public virtual List<BodyHairAndColorsData> HairAndColorsDataSynced { get; set; }
    }
}