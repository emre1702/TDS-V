using Newtonsoft.Json;
using System;
using System.Drawing;

namespace TDS_Common.Dto
{
    public class SyncedTeamDataDto
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public ColorDto Color { get; set; }
        public SyncedTeamPlayerAmountDto AmountPlayers { get; set; }
        public bool IsSpectator => Index == 0;

        public SyncedTeamDataDto(int index, string name, ColorDto color, SyncedTeamPlayerAmountDto amountPlayers)
        {
            Index = index;
            Name = name;
            Color = color;
            AmountPlayers = amountPlayers;
        }
    }
}