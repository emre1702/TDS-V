using System.Xml.Serialization;

namespace TDS_Server.Data.Models.ClothesMeta
{
#nullable disable

    public class Outfits
    {
        #region Public Properties

        [XmlElement("MPOutfitsDataFemale")]
        public OutfitsDataGender OutfitsDataFemale { get; set; }

        [XmlElement("MPOutfitsDataMale")]
        public OutfitsDataGender OutfitsDataMale { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
