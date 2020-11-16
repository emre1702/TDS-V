using System.Collections.Generic;
using System.Xml.Serialization;

namespace TDS.Server.Data.Models.Map
{
#nullable disable warnings

    public class MapObjectsListDto
    {

        [XmlElement("object")]
        public List<MapObjectPosition> Entries { get; set; }

    }

#nullable restore warnings
}
