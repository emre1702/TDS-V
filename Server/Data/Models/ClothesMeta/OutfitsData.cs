using System.Xml.Serialization;

namespace TDS_Server.Data.Models.ClothesMeta
{
#nullable disable

    public class OutfitsData
    {
        #region Public Properties

        [XmlElement("Item")]
        public Item[] Items { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
