using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        public bool HandleBan(PlayerBans? ban)
        {
            if (ban is null)
                return true;

            string startstr = ban.StartTimestamp.ToString(DateTimeFormatInfo.InvariantInfo);
            string endstr = ban.EndTimestamp.HasValue ? ban.EndTimestamp.Value.ToString(DateTimeFormatInfo.InvariantInfo) : "never";
            //todo Test line break and display
            ModPlayer?.Kick($"Banned!\nName: {ban.Player?.Name ?? DisplayName}\nAdmin: {ban.Admin}\nReason: {ban.Reason}\nEnd: {endstr}\nStart: {startstr}");

            return false;
        }
    }
}
