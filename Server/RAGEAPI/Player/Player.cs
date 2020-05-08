using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Models;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Player
{
    class Player : Entity.Entity, IPlayer
    {
        internal readonly GTANetworkAPI.Player _instance;

        private string _name;

        private readonly EntityConvertingHandler _entityConvertingHandler;

        internal Player(GTANetworkAPI.Player instance, EntityConvertingHandler entityConvertingHandler) : base(instance)
        {
            _entityConvertingHandler = entityConvertingHandler;

            _instance = instance;

            _name = _instance.Name;
            SocialClubId = _instance.SocialClubId;
            SocialClubName = _instance.SocialClubName;
            IPAddress = _instance.Address;
            Serial = _instance.Serial;
        }

        public ulong SocialClubId { get; }
        public string SocialClubName { get; }
        public string IPAddress { get; }
        public string Serial { get; }

        public ushort RemoteId => _instance.Handle.Value;
        public int Transparency
        {
            get => _instance.Transparency;
            set => _instance.Transparency = value;
        }
        string IPlayer.Name
        {
            get => _name;
            set 
            {
                _instance.Name = value;
                _name = value;
            }
        }
        public int VehicleSeat => _instance.VehicleSeat;

        public bool Dead => _instance.Dead;

        public WeaponHash CurrentWeapon
        {
            get => (TDS_Shared.Data.Enums.WeaponHash)_instance.CurrentWeapon;
            set => GTANetworkAPI.NAPI.Player.SetPlayerCurrentWeapon(_instance, (GTANetworkAPI.WeaponHash)value);

        }
        public int Armor
        {
            get => _instance.Armor;
            set => _instance.Armor = value;
        }
        public int Health
        {
            get => _instance.Health;
            set => _instance.Health = value;
        }

        public bool IsInVehicle => _instance.IsInVehicle;

        public ITDSVehicle? Vehicle => Init.GetTDSVehicle(_entityConvertingHandler.GetEntity(_instance.Vehicle));

        public bool IsDead => _instance.Dead;

        public string Nametag 
        { 
            get => _instance.Nametag;
            set => _instance.Nametag = value;
        }

        public int Ping => _instance.Ping;

        public bool IsMediaStreamEnabled => _instance.IsMediaStreamEnabled;

        public float Heading 
        { 
            get => _instance.Heading; 
            set => _instance.Heading = value; 
        }
        public Position3D Velocity 
        { 
            get => _instance.Velocity.ToTDS(); 
            set => _instance.Velocity = value.ToMod(); 
        }

        public bool IsCeFenabled => _instance.IsCeFenabled;

        public WeaponHash[] Weapons => _instance.Weapons.Select(w => (WeaponHash)w).ToArray();

        public string Address => _instance.Address;

        public Position3D AimingPoint => _instance.AimingPoint.ToTDS();

        public bool IsAiming => _instance.IsAiming;

        public bool IsShooting => _instance.IsShooting;

        public bool IsReloading => _instance.IsReloading;

        public bool IsInCover => _instance.IsInCover;

        public bool IsOnLadder => _instance.IsOnLadder;

        public HeadBlend HeadBlend 
        { 
            get => _instance.HeadBlend.ToTDS(); 
            set => _instance.HeadBlend = value.ToMod(); 
        }

        public void SetHealth(int health)
        {
            GTANetworkAPI.NAPI.Player.SetPlayerHealth(_instance, health);
        }

        public void SendEvent(string eventName, params object[] args)
        {
            GTANetworkAPI.NAPI.ClientEvent.TriggerClientEvent(_instance, eventName, args);
        }

        public void Spawn(Position3D position, float heading = 0)
        {
            GTANetworkAPI.NAPI.Player.SpawnPlayer(_instance, position.ToMod(), heading);
        }

        public void SetSkin(PedHash pedHash)
        {
            GTANetworkAPI.NAPI.Player.SetPlayerSkin(_instance, (GTANetworkAPI.PedHash)pedHash);
        }

        public void SetVoiceTo(ITDSPlayer target, bool on)
        {
            if (!(target.ModPlayer is Player targetModPlayer))
                return;

            if (on)
                GTANetworkAPI.NAPI.Player.EnablePlayerVoiceTo(_instance, targetModPlayer._instance);
            else
                GTANetworkAPI.NAPI.Player.DisablePlayerVoiceTo(_instance, targetModPlayer._instance);
        }

        public void Kick(string reason)
        {
            GTANetworkAPI.NAPI.Player.KickPlayer(_instance, reason);
        }

        public bool Equals(IPlayer? other)
        {
            return SocialClubId == other?.SocialClubId;
        }

        public void StopAnimation()
        {
            _instance.StopAnimation();
        }

        public void Kill()
        {
            _instance.Kill();
        }

        public void Freeze(bool toggle)
        {
            Init.WorkaroundsHandler.FreezePlayer(_instance, toggle);
        }

        public void SetInvincible(bool toggle)
        {
            Init.WorkaroundsHandler.SetPlayerInvincible(_instance, toggle);
        }

        public void RemoveAllWeapons()
        {
            _instance.RemoveAllWeapons();
        }

        public void GiveWeapon(WeaponHash hash, int ammo = 0)
        {
            _instance.GiveWeapon((GTANetworkAPI.WeaponHash)hash, ammo);
        }

        public void SetWeaponAmmo(WeaponHash hash, int ammo)
        {
            _instance.SetWeaponAmmo((GTANetworkAPI.WeaponHash)hash, ammo);
        }

        public void PlayAnimation(string animDict, string animName, int flag)
        {
            _instance.PlayAnimation(animDict, animName, flag);
        }

        public void SendMessage(string msg)
        {
            _instance.SendChatMessage(msg);
        }

        public void SendNotification(string msg, bool flashing = false)
        {
            _instance.SendNotification(msg, flashing);
        }

        public void WarpOutOfVehicle()
        {
            _instance.WarpOutOfVehicle();
        }

        public void SetIntoVehicle(ITDSVehicle vehicle, int seat)
        {
            if (!(vehicle is Vehicle.Vehicle veh))
                return;
            _instance.SetIntoVehicle(veh._instance, seat);
        }

        public void SetClothes(int slot, int drawable, int texture)
        {
            _instance.SetClothes(slot, drawable, texture);
        }

        public void Ban(string reason)
            => _instance.Ban(reason);

        public void Ban()
            => _instance.Ban();

        public void ClearAccessory(int slot)
            => _instance.ClearAccessory(slot);

        public void ClearDecorations()
            => _instance.ClearDecorations();

        public void Eval(string code)
            => _instance.Eval(code);

        public int GetAccessoryDrawable(int slot)
            => _instance.GetAccessoryDrawable(slot);

        public int GetAccessoryTexture(int slot)
            => _instance.GetAccessoryTexture(slot);

        public int GetClothesDrawable(int slot)
            => _instance.GetClothesDrawable(slot);

        public int GetClothesTexture(int slot)
            => _instance.GetClothesTexture(slot);

        public float GetFaceFeature(int slot)
            => _instance.GetFaceFeature(slot);

        public Color GetHeadBlendPaletteColor(int slot)
            => _instance.GetHeadBlendPaletteColor(slot).ToTDS();

        public HeadOverlay GetHeadOverlay(int overlayId)
            => _instance.GetHeadOverlay(overlayId).ToTDS();

        public T GetOwnSharedData<T>(string key)
            => _instance.GetOwnSharedData<T>(key);

        public int GetWeaponAmmo(WeaponHash weapon)
            => _instance.GetWeaponAmmo((GTANetworkAPI.WeaponHash)weapon);

        public bool HasOwnSharedData(string key)
            => _instance.HasOwnSharedData(key);

        public void Kick()
            => _instance.Kick();

        public void KickSilent(string reason)
            => _instance.KickSilent(reason);

        public void KickSilent()
            => _instance.KickSilent();

        public void PlayScenario(string scenarioName)
            => _instance.PlayScenario(scenarioName);

        public void RemoveDecoration(Decoration decoration)
            => _instance.RemoveDecoration(decoration.ToMod());

        public void RemoveHeadBlendPaletteColor(int slot)
            => _instance.RemoveHeadBlendPaletteColor(slot);

        public void RemoveWeapon(WeaponHash weapon)
            => _instance.RemoveWeapon((GTANetworkAPI.WeaponHash)weapon);

        public void ResetOwnSharedData(string key)
            => _instance.ResetOwnSharedData(key);

        public void SendPictureNotificationToPlayer(string body, string pic, int flash, int iconType, string sender, string subject)
            => _instance.SendPictureNotificationToPlayer(body, pic, flash, iconType, sender, subject);

        public void SetAccessories(int slot, int drawable, int texture)
            => _instance.SetAccessories(slot, drawable, texture);

        public void SetClothes(Dictionary<int, ComponentVariation> clothes)
            => _instance.SetClothes(clothes.ToDictionary(c => c.Key, c => c.Value.ToMod()));

        public void SetCustomization(bool gender, HeadBlend headBlend, byte eyeColor, byte hairColor, byte highlightColor, float[] faceFeatures, 
            Dictionary<int, HeadOverlay> headOverlays, Decoration[] decorations)
            => _instance.SetCustomization(gender, headBlend.ToMod(), eyeColor, hairColor, highlightColor, faceFeatures, 
                headOverlays.ToDictionary(h => h.Key, h => h.Value.ToMod()), decorations.Select(d => d.ToMod()).ToArray());

        public void SetDecoration(Decoration[] decoration)
            => _instance.SetDecoration(decoration.Select(d => d.ToMod()).ToArray());

        public void SetDecoration(Decoration decoration)
            => _instance.SetDecoration(decoration.ToMod());

        public void SetFaceFeature(int slot, float scale)
            => _instance.SetFaceFeature(slot, scale);

        public void SetHeadBlendPaletteColor(int slot, Color color)
            => _instance.SetHeadBlendPaletteColor(slot, color.ToMod());

        public void SetHeadOverlay(int overlayId, HeadOverlay headOverlay)
            => _instance.SetHeadOverlay(overlayId, headOverlay.ToMod());

        public void SetOwnSharedData(Dictionary<string, object> value)
            => _instance.SetOwnSharedData(value);

        public void SetOwnSharedData(string key, object value)
            => _instance.SetOwnSharedData(key, value);

        public void SetSkin(uint newSkin)
            => _instance.SetSkin(newSkin);

        public void TriggerEvent(string eventName, params object[] args)
            => _instance.TriggerEvent(eventName, args);

        public void UpdateHeadBlend(float shapeMix, float skinMix, float thirdMix)
            => _instance.UpdateHeadBlend(shapeMix, skinMix, thirdMix);
    }
}
