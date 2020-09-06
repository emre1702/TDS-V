using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.RAGE.Game.Locale;

namespace TDS_Client.RAGEAPI.Locale
{
    internal class LocaleAPI : ILocaleAPI
    {
        #region Public Methods

        public LanguageID GetCurrentLanguageId()
        {
            return (LanguageID)RAGE.Game.Locale.GetCurrentLanguageId();
        }

        #endregion Public Methods
    }
}