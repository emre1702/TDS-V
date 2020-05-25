using System.Xml.Serialization;

namespace TDS_Server.Data.Models.Map
{
#nullable disable warnings

    public class MapObjectsListDto
    {
        #region Public Properties

        [XmlElement("object")]
        public MapObjectPosition[] Entries { get; set; }

        #endregion Public Properties
    }

#nullable restore warnings
}
