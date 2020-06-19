using System.Collections.Generic;
using System.Drawing;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
#nullable enable

    public interface IPlayer : IEntity
    {
        #region Public Properties

        string Address { get; }
        Position3D AimingPoint { get; }
        int Armor { get; set; }
        WeaponHash CurrentWeapon { get; set; }
        bool Dead { get; }
        public GameTypes GameType { get; }
        HeadBlend HeadBlend { get; set; }
        float Heading { get; set; }
        int Health { get; set; }
        bool IsAiming { get; }
        bool IsCeFenabled { get; }
        bool IsInCover { get; }
        bool IsInVehicle { get; }
        bool IsMediaStreamEnabled { get; }
        bool IsOnLadder { get; }
        bool IsReloading { get; }
        bool IsShooting { get; }
        string Name { get; set; }
        string Nametag { get; set; }
        int Ping { get; }
        string Serial { get; }
        ulong SocialClubId { get; }
        string SocialClubName { get; }
        int Transparency { get; set; }
        IVehicle? Vehicle { get; }
        int VehicleSeat { get; }

        Position3D Velocity { get; set; }

        WeaponHash[] Weapons { get; }

        #endregion Public Properties

        #region Public Methods

        void Ban(string reason);

        void Ban();

        void ClearAccessory(int slot);

        void ClearDecorations();

        void Eval(string code);

        void Freeze(bool toggle);

        int GetAccessoryDrawable(int slot);

        int GetAccessoryTexture(int slot);

        int GetClothesDrawable(int slot);

        int GetClothesTexture(int slot);

        float GetFaceFeature(int slot);

        Color GetHeadBlendPaletteColor(int slot);

        HeadOverlay GetHeadOverlay(int overlayId);

        T GetOwnSharedData<T>(string key);

        int GetWeaponAmmo(WeaponHash weapon);

        void GiveWeapon(WeaponHash hash, int ammo = 0);

        bool HasOwnSharedData(string key);

        void Kick(string reason);

        void Kick();

        void KickSilent(string reason);

        void KickSilent();

        void Kill();

        void PlayAnimation(string animDict, string animName, int flag);

        void PlayScenario(string scenarioName);

        void RemoveAllWeapons();

        void RemoveDecoration(Decoration decoration);

        void RemoveHeadBlendPaletteColor(int slot);

        void RemoveWeapon(WeaponHash weapon);

        void ResetOwnSharedData(string key);

        void SendEvent(string eventName, params object[] args);

        void SendMessage(string msg);

        void SendNotification(string msg, bool flashing = false);

        void SendPictureNotificationToPlayer(string body, string pic, int flash, int iconType, string sender, string subject);

        void SetAccessories(int slot, int drawable, int texture);

        void SetClothes(int slot, int drawable, int texture);

        void SetClothes(Dictionary<int, ComponentVariation> clothes);

        void SetCustomization(bool gender, HeadBlend headBlend, byte eyeColor, byte hairColor, byte highlightColor, float[] faceFeatures,
            Dictionary<int, HeadOverlay> headOverlays, Decoration[] decorations);

        void SetDecoration(Decoration[] decoration);

        void SetDecoration(Decoration decoration);

        void SetFaceFeature(int slot, float scale);

        void SetHeadBlendPaletteColor(int slot, Color color);

        void SetHeadOverlay(int overlayId, HeadOverlay headOverlay);

        void SetHealth(int health);

        public void SetIntoVehicle(IVehicle car, int seat);

        void SetInvincible(bool v);

        void SetOwnSharedData(Dictionary<string, object> value);

        void SetOwnSharedData(string key, object value);

        void SetSkin(PedHash pedHash);

        void SetSkin(uint newSkin);

        void SetVoiceTo(ITDSPlayer target, bool toggle);

        void SetWeaponAmmo(WeaponHash hash, int ammo);

        void Spawn(Position3D position, float heading = 0);

        void StopAnimation();

        void TriggerEvent(string eventName, params object[] args);

        void UpdateHeadBlend(float shapeMix, float skinMix, float thirdMix);

        void WarpOutOfVehicle();

        #endregion Public Methods
    }
}
