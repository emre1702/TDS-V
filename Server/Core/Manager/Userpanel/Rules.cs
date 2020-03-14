using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS_Shared.Data.Enums;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Database.Entity;

namespace TDS_Server.Core.Manager.Userpanel
{
    static class Rules
    {
        private static string _rulesJson = string.Empty;

        public static void LoadRules(TDSDbContext context)
        {
            var rules = context.Rules.Include(r => r.RuleTexts).ToList();
            var sendRules = rules.Select(r => new RuleData 
            {
                Id = r.Id,
                Texts = r.RuleTexts.ToDictionary(t => (int)t.Language, t => t.RuleStr),
                Target = r.Target,
                Category = r.Category
            }).ToList();
            _rulesJson = Serializer.ToBrowser(sendRules);
        }

        public static string GetData()
        {
            return _rulesJson;
        }
    }
    public class RuleData
    {
        [JsonProperty("0")]
        public int Id { get; set; }
        [JsonProperty("1")]
        public Dictionary<int, string>? Texts { get; set; }
        [JsonProperty("2")]
        public ERuleTarget Target { get; set; }
        [JsonProperty("3")]
        public ERuleCategory Category { get; set; }
    }
}
