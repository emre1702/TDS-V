using System.Xml.Serialization;

namespace TDS_Server.Data.Models.Map
{

#nullable disable warnings
    public class MapObjectsListDto
    {
        [XmlElement("object")]
        public MapObjectPosition[] Entries { get; set; }
    }
#nullable restore warnings
}
