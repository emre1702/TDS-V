using System.Xml.Serialization;

namespace TDS_Server.Data.Models.WeaponsMeta
{
    public class WeaponDataValue
    {
        #nullable disable

        [XmlAttribute("value")]
        public float Value { get; set; }
    }
}
