using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Blip
{
    public interface IBlip : IEntity
    {
        #region Public Properties

        Position3D Position { get; set; }
        int Rotation { set; }

        #endregion Public Properties

        #region Public Methods

        void AddTextComponentSubstringName();

        bool DoesExist();

        void EndTextCommandSetName();

        int GetAlpha();

        int GetColour();

        Position3D GetCoords();

        int GetHudColour();

        Position3D GetInfoIdCoord();

        int GetInfoIdDisplay();

        int GetInfoIdEntityIndex();

        int GetInfoIdPickupIndex();

        int GetInfoIdType();

        int GetSprite();

        void HideNumberOn();

        bool IsFlashing();

        bool IsMissionCreator();

        bool IsOnMinimap();

        bool IsShortRange();

        void Pulse();

        void SetAlpha(int alpha);

        void SetAsFriendly(bool toggle);

        void SetAsMissionCreatorBlip(bool toggle);

        void SetAsShortRange(bool toggle);

        void SetBright(bool toggle);

        void SetCategory(int index);

        void SetChecked(bool toggle);

        void SetColour(int color);

        void SetCoords(float posX, float posY, float posZ);

        void SetDisplay(int displayId);

        void SetFade(int opacity, int duration);

        void SetFlashes(bool toggle);

        void SetFlashesAlternate(bool toggle);

        void SetFlashInterval(int p1);

        void SetFlashTimer(int duration);

        void SetFriend(bool toggle);

        void SetFriendly(bool toggle);

        void SetHighDetail(bool toggle);

        void SetNameFromTextFile(string gxtEntry);

        void SetNameToPlayerName(int player);

        void SetPriority(int priority);

        void SetRotation(int rotation);

        void SetRoute(bool enabled);

        void SetRouteColour(int colour);

        void SetScale(float scale);

        void SetSecondaryColour(float r, float g, float b);

        void SetShowCone(bool toggle);

        void SetShrink(bool toggle);

        void SetSprite(int spriteId);

        void ShowHeadingIndicatorOn(bool toggle);

        void ShowNumberOn(int number);

        #endregion Public Methods
    }
}
