using System.Xml.Serialization;

namespace TDS_Server.Dto.ClothesMeta
{
    #nullable disable
    public class OutfitsData
    {
        [XmlElement("Item")]
        public Item[] Items { get; set; }
    }
    #nullable restore
}
