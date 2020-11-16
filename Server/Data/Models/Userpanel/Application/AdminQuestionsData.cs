using Newtonsoft.Json;
using System.Collections.Generic;

namespace TDS.Server.Data.Models.Userpanel.Application
{
    public class AdminQuestionsData
    {
        [JsonProperty("0")]
        public string AdminName { get; set; } 

        [JsonProperty("1")]
        public IEnumerable<AdminQuestionData> Questions { get; set; }
    }
}
