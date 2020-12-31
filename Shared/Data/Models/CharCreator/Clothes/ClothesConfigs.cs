using Newtonsoft.Json;
using System.Collections.Generic;
using TDS.Shared.Data.Models.CharCreator.Clothes;

namespace TDS.Shared.Data.Models.CharCreator
{
    public class ClothesConfigs
    {
        [JsonProperty("0")]
        public List<ClothesData> DatasPerSlot { get; set; }

        [JsonProperty("1")]
        public byte SelectedSlot { get; set; }
    }
}