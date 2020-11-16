using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS.Shared.Data.Models.CharCreator
{
    public class CharCreateData 
    {
        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public virtual List<CharCreateGeneralData> GeneralDataSynced { get; set; }
        [JsonProperty("1")]
        public virtual List<CharCreateHeritageData> HeritageDataSynced { get; set; }
        [JsonProperty("2")]
        public virtual List<CharCreateFeaturesData> FeaturesDataSynced { get; set; }
        [JsonProperty("3")]
        public virtual List<CharCreateAppearanceData> AppearanceDataSynced { get; set; }
        [JsonProperty("4")]
        public virtual List<CharCreateHairAndColorsData> HairAndColorsDataSynced { get; set; }
    }
}
