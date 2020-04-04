using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Ui
{
    public interface IUiAPI
    {
        void SetNotificationTextEntry(string v);
        void AddTextComponentSubstringPlayerName(string msg);
        void DrawNotification(bool v1, bool v2);
        void HideHudComponentThisFrame(HudComponent hudComponent);
    }
}
