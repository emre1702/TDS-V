using MessagePack;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class SyncedTeamPlayerAmountDto
    {
        [Key(0)]
        public uint Amount;
        [Key(1)]
        public uint AmountAlive;
    }
}