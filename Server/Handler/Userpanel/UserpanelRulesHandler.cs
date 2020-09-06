using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using TDS_Server.Database.Entity;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Handler.Userpanel
{
    public class RuleData
    {
        #region Public Properties

        [JsonProperty("3")]
        public RuleCategory Category { get; set; }

        [JsonProperty("0")]
        public int Id { get; set; }

        [JsonProperty("2")]
        public RuleTarget Target { get; set; }

        [JsonProperty("1")]
        public Dictionary<int, string>? Texts { get; set; }

        #endregion Public Properties
    }

    internal class UserpanelRulesHandler
    {
        #region Private Fields

        private string _rulesJson = string.Empty;

        #endregion Private Fields

        #region Public Constructors

        public UserpanelRulesHandler(TDSDbContext dbContext, Serializer serializer)
            => LoadRules(dbContext, serializer);

        #endregion Public Constructors

        #region Public Methods

        public string GetData()
        {
            return _rulesJson;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadRules(TDSDbContext context, Serializer serializer)
        {
            var rules = context.Rules.Include(r => r.RuleTexts).ToList();
            var sendRules = rules.Select(r => new RuleData
            {
                Id = r.Id,
                Texts = r.RuleTexts.ToDictionary(t => (int)t.Language, t => t.RuleStr),
                Target = r.Target,
                Category = r.Category
            }).ToList();
            _rulesJson = serializer.ToBrowser(sendRules);
        }

        #endregion Private Methods
    }
}
