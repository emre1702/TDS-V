using System.Xml.Serialization;

namespace TDS_Server.Data.Models
{
#nullable disable

    public class AppConfigEntryDto
    {
        #region Public Properties

        [XmlAttribute]
        public string Value { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
