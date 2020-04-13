using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Locale;

namespace TDS_Client.RAGEAPI.Locale
{
    class LocaleAPI : ILocaleAPI
    {
        public LanguageID GetCurrentLanguageId()
        {
            return (LanguageID)RAGE.Game.Locale.GetCurrentLanguageId();
        }
    }
}
