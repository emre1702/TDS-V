using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Models
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
        public Dictionary<Language, string> Names { get; } = new Dictionary<Language, string>();
        public List<ITDSPlayer> PlayersOnline { get; } = new List<ITDSPlayer>();

    }
}
