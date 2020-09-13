﻿using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        #region Private Fields

        private TDS_Shared.Data.Enums.Language _langEnumBeforeLogin = TDS_Shared.Data.Enums.Language.English;

        #endregion Private Fields

        #region Public Properties

        public override ILanguage Language { get; set; }

        public override TDS_Shared.Data.Enums.Language LanguageEnum
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
                Language = _langHelper.GetLang(value);
            }
        }

        #endregion Public Properties
    }
}