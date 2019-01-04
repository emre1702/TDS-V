using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TDS_Common.Dto
{
    [Serializable]
    public class SyncedTeamDataDto
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public SyncedTeamPlayerAmountDto AmountPlayers { get; set; }        
    }
}
