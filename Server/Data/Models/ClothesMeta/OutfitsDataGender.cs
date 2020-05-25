using System.Xml.Serialization;

namespace TDS_Server.Data.Models.ClothesMeta
{
#nullable disable

    public class OutfitsDataGender
    {
        #region Public Properties

        [XmlElement("MPOutfitsData")]
        public OutfitsData OutfitsData { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
