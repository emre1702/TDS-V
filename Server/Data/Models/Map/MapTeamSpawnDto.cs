using System.Xml.Serialization;
using TDS_Server.Data.Models.Map.Creator;

namespace TDS_Server.Data.Models.Map
{
#nullable disable warnings

    public class MapTeamSpawnsDto
    {
        #region Public Properties

        [XmlElement("spawn")]
        public Position4DDto[] Spawns { get; set; }

        [XmlIgnore]
        public uint TeamID { get; set; }

        #endregion Public Properties
    }

#nullable restore warnings
}
