using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharDatas 
    {
        #region Public Properties

        [JsonIgnore]
        public int PlayerId { get; set; }
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

        [JsonIgnore]
        public virtual Players Player { get; }
        [JsonIgnore]
        public virtual ICollection<PlayerCharAppearanceDatas> AppearanceData { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayerCharFeaturesDatas> FeaturesData { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayerCharGeneralDatas> GeneralData { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayerCharHairAndColorsDatas> HairAndColorsData { get; set; }
        [JsonIgnore]
        public virtual ICollection<PlayerCharHeritageDatas> HeritageData { get; set; }

        #endregion Public Properties
    }
}
