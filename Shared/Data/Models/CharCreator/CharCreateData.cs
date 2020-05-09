using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.CharCreator
{
    public class CharCreateData 
    {
        [JsonProperty("0")]
        public virtual CharCreateGeneralData GeneralDataSynced { get; set; }
        [JsonProperty("1")]
        public virtual CharCreateHeritageData HeritageDataSynced { get; set; }
        [JsonProperty("2")]
        public virtual CharCreateFeaturesData FeaturesDataSynced { get; set; }
        [JsonProperty("3")]
        public virtual CharCreateAppearanceData AppearanceDataSynced { get; set; }
        [JsonProperty("4")]
        public virtual CharCreateHairAndColorsData HairAndColorsDataSynced { get; set; }
    }
}
