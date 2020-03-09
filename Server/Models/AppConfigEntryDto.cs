using System.Xml.Serialization;

namespace TDS_Server.Dto
{
#nullable disable

    public class AppConfigEntryDto
    {
        [XmlAttribute]
        public string Value { get; set; }
    }

#nullable restore
}