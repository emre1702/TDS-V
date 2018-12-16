using System;
using System.Collections.Generic;
using System.Text;
using TDS.Enum;

namespace TDS.Dto
{
    [Serializable]
    public class MapSynedDataDto
    {
        public string Name = "unknown";
        public EMapType Type = EMapType.Normal;
        public Dictionary<ELanguage, string> Description = new Dictionary<ELanguage, string> {
            [ELanguage.English] = "No info available.",
            [ELanguage.German] = "Keine Info verfügbar."
        };
    }
}
