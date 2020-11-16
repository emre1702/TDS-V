using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS.Server.Data.Models.Userpanel.Application
{
    public class ApplicationUserData
    {
        [JsonProperty("2")]
        public string AdminQuestions { get; set; }

        [JsonIgnore]
        public DateTime CreateDateTime { get; set; }

        [JsonProperty("0")]
        public string CreateTime { get; set; }

        [JsonProperty("1")]
        public IEnumerable<ApplicationUserInvitationData> Invitations { get; set; }
    }
}
