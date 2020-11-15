using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Helper;
using TDS_Shared.Data.Enums;

namespace TDS_Server.PlayersSystem
{
    public class LanguageHandler : IPlayerLanguageHandler
    {
        public ILanguage Data { get; set; }

        public Language Enum
        {
            get
            {
                if (_player.Entity is null || _player.Entity.PlayerSettings is null)
                    return _langEnumBeforeLogin;
                return _player.Entity.PlayerSettings.General.Language;
            }
            set
            {
                if (_player.Entity is null || _player.Entity.PlayerSettings is null)
                    _langEnumBeforeLogin = value;
                else
                    _player.Entity.PlayerSettings.General.Language = value;
                Data = _langHelper.GetLang(value);
            }
        }

        private Language _langEnumBeforeLogin = Language.English;

        private readonly LangHelper _langHelper;

#nullable disable
        private ITDSPlayer _player;
        private IPlayerEvents _events;
#nullable enable

        public LanguageHandler(LangHelper langHelper)
        {
            _langHelper = langHelper;

            Data = _langHelper.GetLang(Language.English);
        }

        public void Init(ITDSPlayer player, IPlayerEvents events)
        {
            _player = player;
            _events = events;

            events.EntityChanged += Events_EntityChanged;
            events.Removed += Events_Removed;
        }

        private void Events_Removed()
        {
            _events.EntityChanged -= Events_EntityChanged;
            _events.Removed -= Events_Removed;
        }

        private void Events_EntityChanged(Players? entity)
        {
            if (entity is null)
                return;

            if (_langEnumBeforeLogin != Language.English)
                entity.PlayerSettings.General.Language = _langEnumBeforeLogin;
            Data = _langHelper.GetLang(entity.PlayerSettings.General.Language);
        }
    }
}
