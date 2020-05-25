using System.Xml.Serialization;

namespace TDS_Server.Data.Models.ClothesMeta
{
#nullable disable

    public class TattooHashes
    {
        #region Public Properties

        [XmlElement("pedTattooHash0")]
        public Component TattooHash0 { get; set; }

        [XmlElement("pedTattooHash1")]
        public Component TattooHash1 { get; set; }

        [XmlElement("pedTattooHash2")]
        public Component TattooHash2 { get; set; }

        #endregion Public Properties
    }

#nullable restore
}
