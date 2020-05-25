using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Ui
{
    public interface IUiAPI
    {
        #region Public Methods

        void AddTextComponentSubstringPlayerName(string text);

        void BeginTextCommandDisplayText(string text);

        bool DoesBlipExist(int blip);

        int DrawNotification(bool blink);

        void EndTextCommandDisplayText(float x, float y);

        float GetTextScaleHeight(float scale, Font font);

        void HideHudAndRadarThisFrame();

        void HideHudComponentThisFrame(HudComponent hudComponent);

        void RemoveBlip(ref int blip);

        void SetBlipAsFriendly(int blip, bool toggle);

        void SetNotificationTextEntry(string type);

        void SetTextCentre(bool align);

        void SetTextColour(int r, int g, int b, int a);

        void SetTextDropShadow();

        void SetTextFont(Font font);

        void SetTextJustification(int justify);

        void SetTextScale(float size);

        #endregion Public Methods

        /**
         * <summary>
         * 0: Center-Justify
         * 1: Left-Justify
         * 2: Right-Justify
         * Right-Justify requires SET_TEXT_WRAP, otherwise it will draw to the far right of the screen
         * </summary>
         */
    }
}
