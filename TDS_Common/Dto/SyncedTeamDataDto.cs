using MessagePack;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class SyncedTeamDataDto
    {
        [Key(0)]
        public int Index { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public ColorDto Color { get; set; }
        [Key(3)]
        public SyncedTeamPlayerAmountDto AmountPlayers { get; set; }
        [Key(4)]
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