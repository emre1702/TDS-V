using System.Globalization;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Core;

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

            ModPlayer?.SendNotification($"Banned!\nName: {ban.Player?.Name ?? DisplayName}\nAdmin: {ban.Admin.Name}\nReason: {ban.Reason}\nEnd: {endstr}\nStart: {startstr}", true);
            new TDSTimer(() =>
            {
                ModPlayer?.Kick("Ban");
            }, 2000, 1);


            return false;
        }
    }
}
