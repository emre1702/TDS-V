using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Ui;

namespace TDS_Client.RAGEAPI.Ui
{
    internal class UiAPI : IUiAPI
    {
        #region Public Methods

        public void AddTextComponentSubstringPlayerName(string text)
        {
            RAGE.Game.Ui.AddTextComponentSubstringPlayerName(text);
        }

        public void BeginTextCommandDisplayText(string text)
        {
            RAGE.Game.Ui.BeginTextCommandDisplayText(text);
        }

        public void DisplayRadar(bool toggle)
            => RAGE.Game.Ui.DisplayRadar(toggle);

        public bool DoesBlipExist(int blip)
        {
            return RAGE.Game.Ui.DoesBlipExist(blip);
        }

        public int DrawNotification(bool blink)
        {
            return RAGE.Game.Ui.DrawNotification(blink, false);
        }

        public void EndTextCommandDisplayText(float x, float y)
        {
            RAGE.Game.Ui.EndTextCommandDisplayText(x, y, 0);
        }

        public float GetTextScaleHeight(float scale, Font font)
        {
            return RAGE.Game.Ui.GetTextScaleHeight(scale, (int)font);
        }

        public void HideHudAndRadarThisFrame()
        {
            RAGE.Game.Ui.HideHudAndRadarThisFrame();
        }

        public void HideHudComponentThisFrame(HudComponent hudComponent)
        {
            RAGE.Game.Ui.HideHudComponentThisFrame((int)hudComponent);
        }

        public void RemoveBlip(ref int blip)
        {
            RAGE.Game.Ui.RemoveBlip(ref blip);
        }

        public void SetBlipAsFriendly(int blip, bool toggle)
        {
            RAGE.Game.Ui.SetBlipAsFriendly(blip, toggle);
        }

        public void SetNotificationTextEntry(string type)
        {
            RAGE.Game.Ui.SetNotificationTextEntry(type);
        }

        public void SetTextCentre(bool align)
        {
            RAGE.Game.Ui.SetTextCentre(align);
        }

        public void SetTextColour(int r, int g, int b, int a)
        {
            RAGE.Game.Ui.SetTextColour(r, g, b, a);
        }

        public void SetTextDropShadow()
        {
            RAGE.Game.Ui.SetTextDropShadow();
        }

        public void SetTextFont(Font font)
        {
            RAGE.Game.Ui.SetTextFont((int)font);
        }

        public void SetTextJustification(int justify)
        {
            RAGE.Game.Ui.SetTextJustification(justify);
        }

        public void SetTextScale(float size)
        {
            RAGE.Game.Ui.SetTextScale(0f, size);
        }

        #endregion Public Methods
    }
}
