using Newtonsoft.Json;
using TDS.Shared.Data.Enums.Userpanel;

namespace TDS.Server.Data.Models.Userpanel.Application
{
    public class AdminQuestionData
    {
        [JsonProperty("2")]
        public UserpanelAdminQuestionAnswerType AnswerType { get; set; }

        [JsonProperty("0")]
        public int ID { get; set; }

        [JsonProperty("1")]
        public string Question { get; set; } = string.Empty;
    }
}
