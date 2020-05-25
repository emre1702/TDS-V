using System.Xml.Serialization;

namespace TDS_Server.Data.Models.Map
{
#nullable disable warnings

    public class MapTeamSpawnsListDto
    {
        #region Public Properties

        [XmlElement("team")]
        public MapTeamSpawnsDto[] TeamSpawns { get; set; }

        #endregion Public Properties
    }

#nullable restore warnings
}
