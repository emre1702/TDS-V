using Newtonsoft.Json;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharDatas : CharCreateData
    {
        #region Public Properties

        [JsonIgnore]
        public virtual PlayerCharAppearanceDatas AppearanceData { get; set; }

        [JsonIgnore]
        public int AppearanceDataId { get; set; }

        [JsonIgnore]
        public virtual PlayerCharFeaturesDatas FeaturesData { get; set; }

        [JsonIgnore]
        public int FeaturesDataId { get; set; }

        [JsonIgnore]
        public virtual PlayerCharGeneralDatas GeneralData { get; set; }

        [JsonIgnore]
        public int GeneralDataId { get; set; }

        [JsonIgnore]
        public virtual PlayerCharHairAndColorsDatas HairAndColorsData { get; set; }

        [JsonIgnore]
        public int HairAndColorsDataId { get; set; }

        [JsonIgnore]
        public virtual PlayerCharHeritageDatas HeritageData { get; set; }

        [JsonIgnore]
        public int HeritageDataId { get; set; }

        [JsonIgnore]
        public virtual Players Player { get; }

        [JsonIgnore]
        public int PlayerId { get; set; }

        #endregion Public Properties
    }
}
