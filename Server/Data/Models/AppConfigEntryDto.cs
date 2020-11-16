using System.Xml.Serialization;

namespace TDS.Server.Data.Models
{
#nullable disable

    public class AppConfigEntryDto
    {
        [XmlAttribute]
        public string Value { get; set; }
    }

#nullable restore
}
