using System.Xml.Serialization;

namespace TDS_Server.Data.Models.ClothesMeta
{
    #nullable disable
    public class Component
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }
    #nullable restore
}
