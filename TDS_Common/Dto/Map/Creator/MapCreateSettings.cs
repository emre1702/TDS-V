using MessagePack;

namespace TDS_Common.Dto.Map.Creator
{
    [MessagePackObject]
    public class MapCreateSettings
    {
        [Key(0)]
        public uint MinPlayers { get; set; }
        [Key(1)]
        public uint MaxPlayers { get; set; }
    }
}
