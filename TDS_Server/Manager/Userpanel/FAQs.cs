using MessagePack;
using System.Collections.Generic;
using System.Linq;
using TDS_Common.Enum;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Userpanel
{
    class FAQs
    {
        private static readonly Dictionary<ELanguage, string> _faqsJsonByLanguage = new Dictionary<ELanguage, string>() 
        { 
            [ELanguage.English] = string.Empty,
            [ELanguage.German] = string.Empty
        };


        public static void LoadFAQs(TDSDbContext dbContext)
        {
            var allFAQs = dbContext.FAQs.ToList();

            foreach (var entry in _faqsJsonByLanguage.ToList())
            {
                var faqs = allFAQs
                    .Where(f => f.Language == entry.Key)
                    .Select(f => new FAQData 
                    {
                        Id = f.Id,
                        Question = f.Question,
                        Answer = f.Answer
                    });
                _faqsJsonByLanguage[entry.Key] = Serializer.ToBrowser(faqs);
            }
        }

        public static string GetData(TDSPlayer player)
        {
            return _faqsJsonByLanguage[player.LanguageEnum];
        }

        [MessagePackObject]
        private class FAQData
        {
            [Key(0)]
            public int Id { get; set; }
            [Key(1)]
            public string Question { get; set; } = string.Empty;
            [Key(2)]
            public string Answer { get; set; } = string.Empty;
        }
    }
}
