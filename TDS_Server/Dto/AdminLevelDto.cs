using System.Collections.Generic;
using TDS_Common.Enum;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Dto
{
    public class AdminLevelDto
    {
        public short Level { get; set; }
        public string FontColor { get; set; }
        public Dictionary<ELanguage, string> Names { get; set; } = new Dictionary<ELanguage, string>();
        public List<TDSPlayer> PlayersOnline { get; set; } = new List<TDSPlayer>();

        public AdminLevelDto(short level, string fontColor)
        {
            Level = level;
            FontColor = fontColor;
        }
    }
}