using System;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Shared.Data.Default;
using TimeZoneConverter;

namespace TDS_Server.PlayersSystem
{
    public class Timezone : IPlayerTimezone
    {
        private TimeZoneInfo _timezone = TimeZoneInfo.Utc;

#nullable disable
        private ITDSPlayer _player;
        private IPlayerEvents _events;
#nullable enable

        public void Init(ITDSPlayer player, IPlayerEvents events)
        {
            _player = player;
            _events = events;

            events.EntityChanged += Events_EntityChanged;
            events.SettingsChanged += Events_SettingsChanged;
            events.Removed += Events_Removed;
        }

        private void Events_Removed()
        {
            _events.EntityChanged -= Events_EntityChanged;
            _events.SettingsChanged -= Events_SettingsChanged;
            _events.Removed -= Events_Removed;
        }

        private void Events_EntityChanged(Database.Entity.Player.Players? entity)
        {
            if (entity is null)
                return;
            LoadTimezone(entity);
        }

        private void Events_SettingsChanged()
        {
            if (_player.Entity is null)
                return;
            LoadTimezone(_player.Entity);
        }

        public DateTime GetLocalDateTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, _timezone);
        }

        public string GetLocalDateTimeString(DateTime dateTime)
        {
            return GetLocalDateTime(dateTime).ToString(_player.Entity?.PlayerSettings.General.DateTimeFormat ?? SharedConstants.DateTimeOffsetFormat);
        }

        private void LoadTimezone(Database.Entity.Player.Players entity)
        {
            _timezone = TZConvert.GetTimeZoneInfo(entity.PlayerSettings.General.Timezone);
        }
    }
}
