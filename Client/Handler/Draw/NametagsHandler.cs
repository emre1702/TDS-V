using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Extensions;
using TDS.Client.Handler.Deathmatch;
using TDS.Shared.Data.Enums;
using static RAGE.Events;

namespace TDS.Client.Handler.Draw
{
    public class NametagsHandler : ServiceBase
    {
        private readonly CamerasHandler _camerasHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly PlayerFightHandler _playerFightHandler;

        public NametagsHandler(LoggingHandler loggingHandler, CamerasHandler camerasHandler, SettingsHandler settingsHandler, UtilsHandler utilsHandler,
            PlayerFightHandler playerFightHandler)
            : base(loggingHandler)
        {
            _camerasHandler = camerasHandler;
            _settingsHandler = settingsHandler;
            _utilsHandler = utilsHandler;
            _playerFightHandler = playerFightHandler;

            Tick += Draw;
        }

        public void Draw(List<TickNametagData> nametags)
        {
            if (GetShowOnlyAtAim())
            {
                DrawAtAim();
                DrawSpectatedNametag();
            }
            else
                DrawAtSight(nametags);
        }

        private bool GetShowOnlyAtAim()
            => _playerFightHandler.InFight && _settingsHandler.ShowNametagOnlyOnAiming;

        public void DrawNametag(int handle, string name, float distance)
        {
            float scale = Math.Min(Math.Max(distance / _settingsHandler.NametagMaxDistance, 0.5f), 0.8f);
            var position = Ped.GetPedBoneCoords(handle, (int)PedBone.IK_Head, 0, 0, 0);
            position.Z += 0.3f + Math.Min(distance / _settingsHandler.NametagMaxDistance, 0.4f);

            float screenX = 0;
            float screenY = 0;
            Graphics.GetScreenCoordFromWorldCoord(position.X, position.Y, position.Z, ref screenX, ref screenY);

            float textheight = Ui.GetTextScaleHeight(scale, (int)Font.ChaletLondon);
            screenY -= textheight;

            RAGE.NUI.UIResText.Draw(name, (int)(1920 * screenX), (int)(1080 * screenY), Font.ChaletLondon, scale, GetHealthColor(handle),
                RAGE.NUI.UIResText.Alignment.Centered, true, true, 0);
        }

        private void DrawAtAim()
        {
            int targetEntity = 0;
            if (!Player.GetEntityPlayerIsFreeAimingAt(ref targetEntity))
                return;

            if (Entity.GetEntityType(targetEntity) != (int)EntityTypeInGetEntityType.Ped)
                return;

            if (!RAGE.Game.Entity.HasEntityClearLosToEntity(RAGE.Elements.Player.LocalPlayer.Handle, targetEntity, 17))
                return;

            var myPos = _camerasHandler.ActiveCamera?.Position ?? RAGE.Elements.Player.LocalPlayer.Position;
            var hisPos = Entity.GetEntityCoords(targetEntity, true);
            var distance = myPos.DistanceTo(hisPos);

            if (distance > _settingsHandler.NametagMaxDistance)
                return;

            string name = "Ped";
            var player = RAGE.Elements.Entities.Players.GetAtHandle(targetEntity) as ITDSPlayer;
            if (!(player is null))
                name = _utilsHandler.GetDisplayName(player);

            if (player is null)
                Logging.LogWarning("GetAtHandle did not work. TargetEntity: " + targetEntity + " | Linq: " + (RAGE.Elements.Entities.Players.All.Any(p => p.Handle == targetEntity)), "NametagsHandler.DrawAtAim");

            DrawNametag(targetEntity, name, distance);
        }

        private void DrawAtSight(List<TickNametagData> nametags)
        {
            if (nametags == null)
                return;
            foreach (var nametag in nametags)
            {
                if (nametag.Distance > _settingsHandler.NametagMaxDistance)
                    continue;

                if (!RAGE.Game.Entity.HasEntityClearLosToEntity(RAGE.Elements.Player.LocalPlayer.Handle, nametag.Player.Handle, 17))
                    continue;

                DrawNametag(nametag.Player.Handle, _utilsHandler.GetDisplayName(nametag.Player as ITDSPlayer), nametag.Distance);
            }
        }

        private void DrawSpectatedNametag()
        {
            if (!(_camerasHandler.Spectating.SpectatingEntity is ITDSPlayer target))
                return;
            if (_camerasHandler.ActiveCamera is null)
                return;

            var myPos = _camerasHandler.ActiveCamera.Position;
            var distance = target.Position.DistanceTo(myPos);

            DrawNametag(target.Handle, target.Name, distance);
        }

        private Color GetArmorColor(int armor)
        {
            if (!_settingsHandler.NametagArmorEmptyColor.HasValue)
                return GetArmorColor(100, armor);

            return _settingsHandler.NametagArmorFullColor.GetBetween(_settingsHandler.NametagArmorEmptyColor.Value, armor / _settingsHandler.StartArmor);
        }

        private Color GetArmorColor(int hp, int armor)
        {
            if (_settingsHandler.NametagArmorEmptyColor.HasValue)
                return GetArmorColor(armor);

            return _settingsHandler.NametagArmorFullColor.GetBetween(GetHpColor(hp), armor / _settingsHandler.StartArmor);
        }

        private Color GetHealthColor(int handle)
        {
            var hp = Math.Max(Entity.GetEntityHealth(handle) - 100, 0);
            var armor = Ped.GetPedArmour(handle);

            //RAGE.Chat.Output($"HP: {hp} - Armor: {armor} - HP orig: {Entity.GetEntityHealth(handle)}");
            return GetHealthColor(hp, armor);
        }

        private Color GetHealthColor(int hp, int armor)
        {
            if (hp == 0)
                return _settingsHandler.NametagDeadColor ?? _settingsHandler.NametagHealthEmptyColor;

            if (armor == 0)
                return GetHpColor(hp);

            if (_settingsHandler.NametagArmorEmptyColor.HasValue)
                return GetArmorColor(armor);
            else
                return GetArmorColor(hp, armor);

            /*if (armor == 0)
                return Color.FromArgb(ClientConstants.NametagAlpha, (int)Math.Ceiling((100 - hp) * 2.55 / 2), (int)Math.Ceiling(hp * 2.55), 0);

            if (armor > 100)
                return Color.FromArgb(ClientConstants.NametagAlpha,
                    (int)Math.Ceiling(255 - hp * 2.55),
                    (int)Math.Ceiling(255 - hp * 2.55 / 2 - (armor - 100) * 2.55 / 2),
                    (int)Math.Ceiling(255 - hp * 2.55 / 2));

            return Color.FromArgb(ClientConstants.NametagAlpha,
                    (int)Math.Ceiling(armor * 2.55),
                    (int)Math.Ceiling(armor * 2.55 / 2 + hp * 2.55 / 2),
                    (int)Math.Ceiling(armor * 2.55));*/
        }

        private Color GetHpColor(int hp)
        {
            return _settingsHandler.NametagHealthFullColor.GetBetween(_settingsHandler.NametagHealthEmptyColor, hp / _settingsHandler.StartHealth);
        }
    }
}
