using System.Xml.Serialization;

namespace TDS_Server.Data.Models
{
    public class EFExtensionsEntryDto
    {
        [XmlAttribute]
        public string Name { get; set; } = string.Empty;

        [XmlAttribute]
        public string Key { get; set; } = string.Empty;
    }
}
