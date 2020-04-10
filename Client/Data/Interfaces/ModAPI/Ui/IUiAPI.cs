using System;
using System.Collections.Generic;
using System.Drawing;
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
        float GetTextScaleHeight(float scale, Font font);
        void HideHudAndRadarThisFrame();
        void BeginTextCommandDisplayText(string v);
        void SetTextScale(float v1, float v2);
        void SetTextColour(int v1, int v2, int v3, int v4);
        void SetTextCentre(bool v);
        void SetTextJustification(int v);
        void SetTextFont(int v);
        void SetTextDropShadow();
        void EndTextCommandDisplayText(float v1, float v2, int v3);
    }
}
