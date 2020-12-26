using Newtonsoft.Json;

namespace TDS.Shared.Data.Models.CharCreator.Body
{
    public class BodyGeneralData
    {
        [JsonProperty("99")]
        public byte Slot { get; set; }

        [JsonProperty("0")]
        public bool IsMale { get; set; }
    }
}