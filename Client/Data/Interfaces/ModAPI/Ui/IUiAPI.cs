using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Blip;

namespace TDS_Client.Data.Interfaces.ModAPI.Ui
{
    public interface IUiAPI
    {
        void SetNotificationTextEntry(string type);
        void AddTextComponentSubstringPlayerName(string text);
        int DrawNotification(bool blink);
        void HideHudComponentThisFrame(HudComponent hudComponent);
        float GetTextScaleHeight(float scale, Font font);
        void HideHudAndRadarThisFrame();
        void BeginTextCommandDisplayText(string text);
        void SetTextScale(float size);
        void SetTextColour(int r, int g, int b, int a);
        void SetTextCentre(bool align);

        /**
         * <summary>
         * 0: Center-Justify 
         * 1: Left-Justify 
         * 2: Right-Justify 
         * Right-Justify requires SET_TEXT_WRAP, otherwise it will draw to the far right of the screen
         * </summary>
         */
        void SetTextJustification(int justify);
        void SetTextFont(Font font);
        void SetTextDropShadow();
        void EndTextCommandDisplayText(float x, float y);
        void SetBlipAsFriendly(int blip, bool toggle);
        bool DoesBlipExist(int blip);
        void RemoveBlip(ref int blip);
    }
}
