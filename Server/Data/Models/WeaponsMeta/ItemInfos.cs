using System.Xml.Serialization;

namespace TDS_Server.Data.Models.WeaponsMeta
{
    public class ItemInfos
    {
        #nullable disable 

        [XmlElement("Infos")]
        public ItemInfos2 ItemInfos2 { get; set; }
    }
}
