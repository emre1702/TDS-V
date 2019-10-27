using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Userpanel
{
    class Rules
    {
        #nullable disable warnings
        private static string _rulesJson;
        #nullable restore warnings

        public static void LoadRules(TDSNewContext context)
        {
            var rules = context.Rules.Include(r => r.RuleTexts).ToList();
            var sendRules = rules.Select(r => new {
               r.Id,
               Texts = r.RuleTexts.ToDictionary(t => (int)t.Language, t => t.RuleStr),
               r.Target,
               r.Category
            });
            _rulesJson = JsonConvert.SerializeObject(sendRules);
        }

        public static string GetData(TDSPlayer player)
        {
            return _rulesJson;
        }
    }
}
