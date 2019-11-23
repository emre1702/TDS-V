using MessagePack;
using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Common.Dto
{
    [MessagePackObject]
    public class SyncedMapDataDto
    {
        [Key(0)]
        public int Id = 0;
        [Key(1)]
        public string Name = "unknown";
        [Key(2)]
        public EMapType Type = EMapType.Normal;
        [Key(3)]
        public Dictionary<int, string> Description = new Dictionary<int, string>
        {
            [(int)ELanguage.English] = "No info available.",
            [(int)ELanguage.German] = "Keine Info verfügbar."
        };
        [Key(4)]
        public string CreatorName;
        [Key(5)]
        public uint Rating = 5;
    }
}