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
        public Dictionary<ELanguage, string> Description = new Dictionary<ELanguage, string> {
            [ELanguage.English] = "No info available.",
            [ELanguage.German] = "Keine Info verfügbar."
        };
        public string CreatorName;
    }
}
