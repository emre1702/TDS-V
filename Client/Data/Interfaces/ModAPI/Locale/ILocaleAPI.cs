using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Locale
{
    public interface ILocaleAPI
    {
        #region Public Methods

        LanguageID GetCurrentLanguageId();

        #endregion Public Methods
    }
}
