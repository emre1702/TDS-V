using Newtonsoft.Json;
using System;
using System.Drawing;

namespace TDS_Common.Dto
{
    [Serializable]
    public class SyncedTeamDataDto
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public ColorDto Color { get; set; }
        public SyncedTeamPlayerAmountDto AmountPlayers { get; set; }


        public SyncedTeamDataDto(int index, string name, ColorDto color, SyncedTeamPlayerAmountDto amountPlayers)
        {
            Index = index;
            Name = name;
            Color = color;
            AmountPlayers = amountPlayers;
        }
    }
}