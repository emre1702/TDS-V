using System.Collections.Generic;
using System.Xml.Serialization;

namespace TDS_Server.Data.Models.WeaponsMeta
{
    public class ItemInfos2
    {
        #nullable disable

        [XmlElement("Item")]
        public List<WeaponData> Data { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}
