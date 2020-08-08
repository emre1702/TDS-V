using Newtonsoft.Json;
using System.Collections.Generic;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharDatas : CharCreateData
    {
        #region Public Properties

        [JsonIgnore]
        public int PlayerId { get; set; }


        [JsonIgnore]
        public virtual ICollection<Players> Player { get; }
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
