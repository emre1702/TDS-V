using Newtonsoft.Json;

namespace TDS_Shared.Data.Models.CharCreator
{
    public class CharCreateGeneralData
    {
        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public bool IsMale { get; set; }
    }
}
