using MessagePack;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Userpanel
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

        public static string GetData(TDSPlayer player)
        {
            return _rulesJson;
        }
    }

    [MessagePackObject]
    public class RuleData
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public Dictionary<int, string>? Texts { get; set; }
        [Key(2)]
        public ERuleTarget Target { get; set; }
        [Key(3)]
        public ERuleCategory Category { get; set; }
    }
}
