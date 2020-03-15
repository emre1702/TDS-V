using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TDS_Shared.Data.Enums;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Database.Entity;

namespace TDS_Server.Core.Manager.Userpanel
{
    class FAQs
    {
        private static readonly Dictionary<Language, string> _faqsJsonByLanguage = new Dictionary<Language, string>() 
        { 
            [Language.English] = string.Empty,
            [Language.German] = string.Empty
        };


        public static void LoadFAQs(TDSDbContext dbContext)
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
                _faqsJsonByLanguage[entry.Key] = Serializer.ToBrowser(faqs);
            }
        }

        public static string GetData(TDSPlayer player)
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
