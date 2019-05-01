using System.Collections.Generic;
using TDS_Common.Enum;
using TDS_Server.Instance.Player;

namespace TDS_Server.Dto
{
    internal class AdminLevelDto
    {
        public byte Level;
        public string FontColor;
        public Dictionary<ELanguage, string> Names = new Dictionary<ELanguage, string>();
        public List<TDSPlayer> PlayersOnline = new List<TDSPlayer>();

        public AdminLevelDto(byte level, string fontColor)
        {
            Level = level;
            FontColor = fontColor;
        }
    }
}