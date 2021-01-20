using System.Collections.Generic;
using System.Xml.Serialization;

namespace TDS.Server.Data.Models.AppConfig
{
#nullable disable

    [XmlRoot("root")]
    public class AppConfigDto
    {
        [XmlElement]
        public AppConfigEntryDto ConnectionString { get; set; }

        [XmlElement]
        public AppConfigEntryDto GitHubToken { get; set; }

        [XmlArray("Logging")]
        [XmlArrayItem("File")]
        public List<AppConfigLoggingSetting> Logging { get; set; }
    }

#nullable restore
}