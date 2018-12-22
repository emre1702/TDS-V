using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Enum;

namespace TDS_Server.Dto
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
