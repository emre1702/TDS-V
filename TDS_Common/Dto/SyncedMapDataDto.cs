using System;
using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Common.Dto
{
    [Serializable]
    public class SyncedMapDataDto
    {
        public string Name = "unknown";
        public EMapType Type = EMapType.Normal;
        public Dictionary<int, string?> Description = new Dictionary<int, string?> {
            [(int)ELanguage.English] = "No info available.",
            [(int)ELanguage.German] = "Keine Info verfügbar."
        };
        public string CreatorName = "unknown";
    }
}
