using System;
using System.Globalization;
using TDS_Common.Manager.Utility;
using TimeZoneConverter;

namespace TDS_Server.Instance.PlayerInstance
{
    partial class TDSPlayer
    {
        private TimeZoneInfo _timezone = TimeZoneInfo.Utc;

        public void LoadTimezone()
        {
            if (Entity == null)
                return;
            _timezone = TZConvert.GetTimeZoneInfo(Entity.PlayerSettings.Timezone);
        }

        public DateTime GetLocalDateTime(DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTime(dateTime, _timezone);
        }

        public string GetLocalDateTimeString(DateTime dateTime)
        {
            return GetLocalDateTime(dateTime).ToString(Entity?.PlayerSettings.DateTimeFormat ?? Constants.DateTimeOffsetFormat);
        }
    }
}
