using System.Collections.Generic;
using TDS_Server.Database.Interfaces;
using TDS_Shared.Data.Models.CharCreator;

namespace TDS_Server.Database.Entity.Player.Char
{
    public class PlayerCharDatas : IPlayerDataTable
    {
        public int PlayerId { get; set; }
        public byte Slot { get; set; }
        public CharCreateData SyncedData { get; set; }


        public virtual Players Player { get; }
        public virtual ICollection<PlayerCharAppearanceDatas> AppearanceData { get; set; }
        public virtual ICollection<PlayerCharFeaturesDatas> FeaturesData { get; set; }
        public virtual ICollection<PlayerCharGeneralDatas> GeneralData { get; set; }
        public virtual ICollection<PlayerCharHairAndColorsDatas> HairAndColorsData { get; set; }
        public virtual ICollection<PlayerCharHeritageDatas> HeritageData { get; set; }
    }
}
