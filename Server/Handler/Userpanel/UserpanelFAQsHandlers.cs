using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Shared.Data.Enums;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Userpanel
{
    class UserpanelFAQsHandlers
    {
        private readonly Dictionary<Language, string> _faqsJsonByLanguage = new Dictionary<Language, string>()
        {
            [Language.English] = string.Empty,
            [Language.German] = string.Empty
        };

        public UserpanelFAQsHandlers(TDSDbContext dbContext, Serializer serializer)
        {
            LoadFAQs(dbContext, serializer);
        }

        public void LoadFAQs(TDSDbContext dbContext, Serializer serializer)
        {
            Regex regex = new Regex("(?<=(\r\n|\r|\n))[ ]{2,}", RegexOptions.None);

            var allFAQs = dbContext.FAQs.ToList();

            foreach (var entry in _faqsJsonByLanguage.ToList())
            {
                var faqs = allFAQs
                    .Where(f => f.Language == entry.Key)
                    .Select(f => new FAQData
                    {
                        Id = f.Id,
                        Question = regex.Replace(f.Question, ""),
                        Answer = regex.Replace(f.Answer, "")
                    });
                _faqsJsonByLanguage[entry.Key] = serializer.ToBrowser(faqs);
            }
        }

        public string GetData(ITDSPlayer player)
        {
            return _faqsJsonByLanguage[player.LanguageEnum];
        }
    }

    public class FAQData
    {
        [JsonProperty("0")]
        public int Id { get; set; }
        [JsonProperty("1")]
        public string Question { get; set; } = string.Empty;
        [JsonProperty("2")]
        public string Answer { get; set; } = string.Empty;
    }
}
