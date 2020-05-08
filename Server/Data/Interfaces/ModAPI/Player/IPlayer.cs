using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.ModAPI.Ped;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Player
{
    #nullable enable
    public interface IPlayer : IPedBase
    {
        bool Dead { get; }
        string IPAddress { get; }
        string Name { get; set; }
        ushort RemoteId { get; }

        void SetInvincible(bool v);

        string Serial { get; }
        ulong SocialClubId { get; }
        string SocialClubName { get; }
        int Transparency { get; set; }   

        void RemoveAllWeapons();

        WeaponHash CurrentWeapon { get; set; }

        int Armor { get; set; }
        int Health { get; set; }
        bool IsInVehicle { get; }
        ITDSVehicle? Vehicle { get; }
        bool IsDead { get; }

        void Kick(string reason);
        void SendEvent(string eventName, params object[] args);
        void SetHealth(int health);
        void SetSkin(PedHash pedHash);
        void SetVoiceTo(ITDSPlayer target, bool toggle);
        void Spawn(Position3D position, float heading = 0);
        void GiveWeapon(WeaponHash hash, int ammo = 0);
        void SetWeaponAmmo(WeaponHash hash, int ammo);
        void StopAnimation();
        void Kill();
        void PlayAnimation(string animDict, string animName, int flag);
        void Freeze(bool toggle);
        void SendMessage(string msg);
        void SendNotification(string msg, bool flashing = false);
        void WarpOutOfVehicle();
        void SetIntoVehicle(ITDSVehicle vehicle, int seat);
        void SetClothes(int slot, int drawable, int texture);

        string Nametag { get; set; }
        int Ping { get; }
        bool IsMediaStreamEnabled { get; }
        float Heading { get; set; }
        Position3D Velocity { get; set; }
        bool IsCeFenabled { get; }
        WeaponHash[] Weapons { get; }
        string Address { get; }
        Position3D AimingPoint { get; }
        bool IsAiming { get; }
        bool IsShooting { get; }
        bool IsReloading { get; }
        bool IsInCover { get; }
        bool IsOnLadder { get; }
        HeadBlend HeadBlend { get; set; }

        void Ban(string reason);
        void Ban();
        void ClearAccessory(int slot);
        void ClearDecorations();
        void Eval(string code);
        int GetAccessoryDrawable(int slot);
        int GetAccessoryTexture(int slot);
        int GetClothesDrawable(int slot);
        int GetClothesTexture(int slot);
        float GetFaceFeature(int slot);
        Color GetHeadBlendPaletteColor(int slot);
        HeadOverlay GetHeadOverlay(int overlayId);
        T GetOwnSharedData<T>(string key);
        int GetWeaponAmmo(WeaponHash weapon);
        bool HasOwnSharedData(string key);
        void Kick();
        void KickSilent(string reason);
        void KickSilent();
        void PlayScenario(string scenarioName);
        void RemoveDecoration(Decoration decoration);
        void RemoveHeadBlendPaletteColor(int slot);
        void RemoveWeapon(WeaponHash weapon);
        void ResetOwnSharedData(string key);
        void SendPictureNotificationToPlayer(string body, string pic, int flash, int iconType, string sender, string subject);
        void SetAccessories(int slot, int drawable, int texture);
        void SetClothes(Dictionary<int, ComponentVariation> clothes);
        void SetCustomization(bool gender, HeadBlend headBlend, byte eyeColor, byte hairColor, byte highlightColor, float[] faceFeatures, 
            Dictionary<int, HeadOverlay> headOverlays, Decoration[] decorations);
        void SetDecoration(Decoration[] decoration);
        void SetDecoration(Decoration decoration);
        void SetFaceFeature(int slot, float scale);
        void SetHeadBlendPaletteColor(int slot, Color color);
        void SetHeadOverlay(int overlayId, HeadOverlay headOverlay);
        void SetOwnSharedData(Dictionary<string, object> value);
        void SetOwnSharedData(string key, object value);
        void SetSkin(uint newSkin);
        void TriggerEvent(string eventName, params object[] args);
        void UpdateHeadBlend(float shapeMix, float skinMix, float thirdMix);
    }
}
