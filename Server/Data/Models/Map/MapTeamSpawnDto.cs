using System.Collections.Generic;
using System.Xml.Serialization;
using TDS.Server.Data.Models.Map.Creator;

namespace TDS.Server.Data.Models.Map
{
#nullable disable warnings

    public class MapTeamSpawnsDto
    {
        #region Public Properties

        [XmlElement("spawn")]
        public List<Position4DDto> Spawns { get; set; }

        [XmlIgnore]
        public uint TeamID { get; set; }

        #endregion Public Properties
    }

#nullable restore warnings
}
