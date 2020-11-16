using System.Xml.Serialization;

namespace TDS.Server.Data.Models.ClothesMeta
{
#nullable disable

    public class OutfitsDataGender
    {
        [XmlElement("MPOutfitsData")]
        public OutfitsData OutfitsData { get; set; }

    }
}
