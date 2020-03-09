using System.Xml.Serialization;

namespace TDS_Server.Dto.ClothesMeta
{
    #nullable disable
    public class OutfitsDataGender
    {
        [XmlElement("MPOutfitsData")]
        public OutfitsData OutfitsData { get; set; }
    }
    #nullable restore
}
