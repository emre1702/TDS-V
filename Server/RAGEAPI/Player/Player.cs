using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Data.Models;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Player
{
    internal class Player : GTANetworkAPI.Player, IPlayer
    {
        #region Public Constructors

        public Player(GTANetworkAPI.NetHandle netHandle) : base(netHandle)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public new Position3D AimingPoint => base.AimingPoint.ToTDS();

        public new WeaponHash CurrentWeapon
        {
            get => (WeaponHash)base.CurrentWeapon;
            set => GTANetworkAPI.NAPI.Player.SetPlayerCurrentWeapon(this, (GTANetworkAPI.WeaponHash)value);
        }

        public new GameTypes GameType => (GameTypes)base.GameType;

        public new HeadBlend HeadBlend
        {
            get => base.HeadBlend.ToTDS();
            set => base.HeadBlend = value.ToMod();
        }

        public new Position3D Position
        {
            get => new Position3D(base.Position.X, base.Position.Y, base.Position.Z);
            set => base.Position = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }

        public ushort RemoteId => Handle.Value;

        public new Position3D Rotation
        {
            get => new Position3D(base.Rotation.X, base.Rotation.Y, base.Rotation.Z);
            set => base.Rotation = new GTANetworkAPI.Vector3(value.X, value.Y, value.Z);
        }

        public new IVehicle? Vehicle => base.Vehicle as IVehicle;

        public new Position3D Velocity
        {
            get => base.Velocity.ToTDS();
            set => base.Velocity = value.ToMod();
        }

        public new WeaponHash[] Weapons => base.Weapons.Select(w => (WeaponHash)w).ToArray();

        #endregion Public Properties

        #region Public Methods

        public void AttachTo(ITDSPlayer player, PedBone bone, Position3D? positionOffset, Position3D? rotationOffset)
        {
            if (!(player.ModPlayer is Player modPlayer))
                return;
            var positionOffsetVector = positionOffset?.ToMod() ?? new GTANetworkAPI.Vector3();
            var rotationOffsetVector = rotationOffset?.ToMod() ?? new GTANetworkAPI.Vector3();
            Init.WorkaroundsHandler.AttachEntityToEntity(this, modPlayer, bone, positionOffsetVector, rotationOffsetVector, player.Lobby);
        }

        public void Detach()
        {
            Init.WorkaroundsHandler.DetachEntity(this);
        }

        public bool Equals([AllowNull] IEntity other)
        {
            return Id == other?.Id;
        }

        public void Freeze(bool toggle)
        {
            Init.WorkaroundsHandler.FreezePlayer(this, toggle);
        }

        public void Freeze(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.FreezeEntity(this, toggle, lobby);
        }

        public new Color GetHeadBlendPaletteColor(int slot)
            => base.GetHeadBlendPaletteColor(slot).ToTDS();

        public new HeadOverlay GetHeadOverlay(int overlayId)
            => base.GetHeadOverlay(overlayId).ToTDS();

        public int GetWeaponAmmo(WeaponHash weapon)
            => GetWeaponAmmo((GTANetworkAPI.WeaponHash)weapon);

        public void GiveWeapon(WeaponHash hash, int ammo = 0)
            => GiveWeapon((GTANetworkAPI.WeaponHash)hash, ammo);

        public void RemoveDecoration(Decoration decoration)
            => RemoveDecoration(decoration.ToMod());

        public void RemoveWeapon(WeaponHash weapon)
            => RemoveWeapon((GTANetworkAPI.WeaponHash)weapon);

        public void SendEvent(string eventName, params object[] args)
        {
            GTANetworkAPI.NAPI.ClientEvent.TriggerClientEvent(this, eventName, args);
        }

        public void SendMessage(string msg)
            => SendChatMessage(msg);

        public void SetClothes(Dictionary<int, ComponentVariation> clothes)
            => SetClothes(clothes.ToDictionary(c => c.Key, c => c.Value.ToMod()));

        public void SetCollisionsless(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.SetEntityCollisionless(this, toggle, lobby);
        }

        public void SetCustomization(bool gender, HeadBlend headBlend, byte eyeColor, byte hairColor, byte highlightColor, float[] faceFeatures,
            Dictionary<int, HeadOverlay> headOverlays, Decoration[] decorations)
            => SetCustomization(gender, headBlend.ToMod(), eyeColor, hairColor, highlightColor, faceFeatures,
                headOverlays.ToDictionary(h => h.Key, h => h.Value.ToMod()), decorations.Select(d => d.ToMod()).ToArray());

        public void SetDecoration(Decoration[] decoration)
            => SetDecoration(decoration.Select(d => d.ToMod()).ToArray());

        public void SetDecoration(Decoration decoration)
            => SetDecoration(decoration.ToMod());

        public void SetHeadBlendPaletteColor(int slot, Color color)
            => SetHeadBlendPaletteColor(slot, color.ToMod());

        public void SetHeadOverlay(int overlayId, HeadOverlay headOverlay)
            => SetHeadOverlay(overlayId, headOverlay.ToMod());

        public void SetHealth(int health)
        {
            GTANetworkAPI.NAPI.Player.SetPlayerHealth(this, health);
        }

        public void SetIntoVehicle(IVehicle car, int seat)
            => SetIntoVehicle(car as GTANetworkAPI.Vehicle, seat);

        public void SetInvincible(bool toggle)
        {
            Init.WorkaroundsHandler.SetPlayerInvincible(this, toggle);
        }

        public void SetInvincible(bool toggle, ITDSPlayer forPlayer)
        {
            if (!(forPlayer.ModPlayer is Player modPlayer))
                return;
            Init.WorkaroundsHandler.SetEntityInvincible(modPlayer, this, toggle);
        }

        public void SetInvincible(bool toggle, ILobby lobby)
        {
            Init.WorkaroundsHandler.SetEntityInvincible(lobby, this, toggle);
        }

        public void SetSkin(PedHash pedHash)
        {
            GTANetworkAPI.NAPI.Player.SetPlayerSkin(this, (GTANetworkAPI.PedHash)pedHash);
        }

        public void SetVoiceTo(ITDSPlayer target, bool on)
        {
            if (!(target.ModPlayer is Player targetPlayer))
                return;
            if (on)
                GTANetworkAPI.NAPI.Player.EnablePlayerVoiceTo(this, targetPlayer);
            else
                GTANetworkAPI.NAPI.Player.DisablePlayerVoiceTo(this, targetPlayer);
        }

        public void SetWeaponAmmo(WeaponHash hash, int ammo)
            => SetWeaponAmmo((GTANetworkAPI.WeaponHash)hash, ammo);

        public void Spawn(Position3D position, float heading = 0)
        {
            GTANetworkAPI.NAPI.Player.SpawnPlayer(this, position.ToMod(), heading);
        }

        #endregion Public Methods
    }
}
