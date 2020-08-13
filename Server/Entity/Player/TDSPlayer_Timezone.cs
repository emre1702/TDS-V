using System;
using TDS_Shared.Data.Default;
using TimeZoneConverter;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Private Fields

        private TimeZoneInfo _timezone = TimeZoneInfo.Utc;

        #endregion Private Fields

        #region Public Methods

        public DateTime GetLocalDateTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, _timezone);
        }

        public string GetLocalDateTimeString(DateTime dateTime)
        {
            return GetLocalDateTime(dateTime).ToString(Entity?.PlayerSettings.DateTimeFormat ?? SharedConstants.DateTimeOffsetFormat);
        }

        public void LoadTimezone()
        {
            if (Entity == null)
                return;
            _timezone = TZConvert.GetTimeZoneInfo(Entity.PlayerSettings.Timezone);
        }

        #endregion Public Methods
    }
}
