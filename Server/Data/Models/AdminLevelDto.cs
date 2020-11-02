using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Models
{
    public class AdminLevelDto
    {
        public AdminLevelDto(short level, string fontColor)
        {
            Level = level;
            FontColor = fontColor;
        }

        public string FontColor { get; set; }
        public short Level { get; set; }
        public Dictionary<Language, string> Names { get; set; } = new Dictionary<Language, string>();
        public List<ITDSPlayer> PlayersOnline { get; set; } = new List<ITDSPlayer>();

    }
}
