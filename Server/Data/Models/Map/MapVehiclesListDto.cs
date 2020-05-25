using System.Xml.Serialization;

namespace TDS_Server.Data.Models.Map
{
#nullable disable warnings

    public class MapVehiclesListDto
    {
        #region Public Properties

        [XmlElement("vehicle")]
        public MapObjectPosition[] Entries { get; set; }

        #endregion Public Properties
    }

#nullable restore warnings
}
