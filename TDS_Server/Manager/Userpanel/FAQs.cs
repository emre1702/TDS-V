﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TDS_Common.Enum;
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
                    .Select(f => new {
                        f.Id,
                        f.Question,
                        f.Answer
                    });
                _faqsJsonByLanguage[entry.Key] = JsonConvert.SerializeObject(faqs);
            }
        }

        public static string GetData(TDSPlayer player)
        {
            return _faqsJsonByLanguage[player.LanguageEnum];
        }
    }
}
