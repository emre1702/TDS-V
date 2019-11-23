using MessagePack;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class MapVoteDto
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public int AmountVotes { get; set; }

    }
}
