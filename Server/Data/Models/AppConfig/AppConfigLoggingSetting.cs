using System.Xml.Serialization;

namespace TDS.Server.Data.Models.AppConfig
{
    public class AppConfigLoggingSetting
    {
        [XmlAttribute("Level")]
        public string Level { get; set; }

        [XmlAttribute("Path")]
        public string Path { get; set; }
    }
}