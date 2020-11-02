using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Models.Userpanel.Faq;
using TDS_Server.Database.Entity;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Userpanel
{
    internal class UserpanelFAQsHandlers
    {

        private readonly Dictionary<Language, string> _faqsJsonByLanguage = new Dictionary<Language, string>()
        {
            [Language.English] = string.Empty,
            [Language.German] = string.Empty
        };

        public UserpanelFAQsHandlers(TDSDbContext dbContext)
            => LoadFAQs(dbContext);

        public string GetData(ITDSPlayer player)
        {
            return _faqsJsonByLanguage[player.LanguageHandler.Enum];
        }

        public void LoadFAQs(TDSDbContext dbContext)
        {
            var regex = new Regex("(?<=(\r\n|\r|\n))[ ]{2,}", RegexOptions.None);

            var allFAQs = dbContext.FAQs.ToList();

            foreach (var entry in _faqsJsonByLanguage.ToList())
            {
                var faqs = allFAQs
                    .Where(f => f.Language == entry.Key)
                    .Select(f => new FaqData
                    {
                        Id = f.Id,
                        Question = regex.Replace(f.Question, ""),
                        Answer = regex.Replace(f.Answer, "")
                    });
                _faqsJsonByLanguage[entry.Key] = Serializer.ToBrowser(faqs);
            }
        }

    }
}
