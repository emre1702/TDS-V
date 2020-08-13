using System.Xml.Serialization;
using TDS_Server.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map
{
#nullable disable warnings

    public class MapBombInfoDto
    {
        #region Public Properties

        [XmlElement("plantpos")]
        public PositionDto[] PlantPositions { get; set; }

        [XmlIgnore]
        public string PlantPositionsJson { get; set; }

        #endregion Public Properties
    }

#nullable restore warnings
}
