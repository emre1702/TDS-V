using System.Collections.Generic;
using System.Xml.Serialization;

namespace TDS.Server.Data.Models.ClothesMeta
{
#nullable disable

    public class OutfitsData
    {
        [XmlElement("Item")]
        public List<Item> Items { get; set; }
    }
}
