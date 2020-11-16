using Microsoft.EntityFrameworkCore;
using System.Linq;
using TDS.Server.Data.Models.Userpanel.Rules;
using TDS.Server.Database.Entity;
using TDS.Shared.Core;

namespace TDS.Server.Handler.Userpanel
{
    internal class UserpanelRulesHandler
    {
        private string _rulesJson = string.Empty;

        public UserpanelRulesHandler(TDSDbContext dbContext)
            => LoadRules(dbContext);

        public string GetData()
        {
            return _rulesJson;
        }

        private void LoadRules(TDSDbContext context)
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
    }
}
