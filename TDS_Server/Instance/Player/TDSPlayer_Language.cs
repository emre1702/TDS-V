using TDS_Common.Enum;
using TDS_Server.Interfaces;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Player
{
    partial class TDSPlayer
    {
        private ELanguage _langEnumBeforeLogin = ELanguage.English;

        public ILanguage Language { get; private set; } = LangUtils.GetLang(ELanguage.English);

        public ELanguage LanguageEnum
        {
            get
            {
                if (Entity is null || Entity.PlayerSettings is null)
                    return _langEnumBeforeLogin;
                return Entity.PlayerSettings.Language;
            }
            set
            {
                if (Entity is null || Entity.PlayerSettings is null)
                    _langEnumBeforeLogin = value;
                else
                    Entity.PlayerSettings.Language = value;
                LangUtils.GetLang(value);
            }
        }

    }
}
