using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Extensions;
using static RAGE.Events;

namespace TDS_Client.Handler.Draw
{
    public class NametagsHandler : ServiceBase
    {
        private readonly CamerasHandler _camerasHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly UtilsHandler _utilsHandler;

        public NametagsHandler(LoggingHandler loggingHandler, CamerasHandler camerasHandler, SettingsHandler settingsHandler, UtilsHandler utilsHandler)
            : base(loggingHandler)
        {
            _camerasHandler = camerasHandler;
            _settingsHandler = settingsHandler;
            _utilsHandler = utilsHandler;

            Tick += Draw;
        }

        public void Draw(List<TickNametagData> nametags)
        {
            if (_settingsHandler.ShowNametagOnlyOnAiming)
            {
                DrawAtAim();
                DrawSpectatedNametag();
            }
            else
                DrawAtSight(nametags);
        }

        public void DrawNametag(int handle, string name, float distance)
        {
            float scale = Math.Max(distance / _settingsHandler.NametagMaxDistance, 0.5f);
            var position = RAGE.Game.Entity.GetEntityCoords(handle, true);
            position.Z += 0.9f + distance / _settingsHandler.NametagMaxDistance;

            float screenX = 0;
            float screenY = 0;
            RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(position.X, position.Y, position.Z, ref screenX, ref screenY);

            float textheight = RAGE.Game.Ui.GetTextScaleHeight(scale, (int)Font.ChaletLondon);
            screenY -= textheight;

            RAGE.NUI.UIResText.Draw(name, (int)(1920 * screenX), (int)(1080 * screenY), Font.ChaletLondon, scale, GetHealthColor(handle),
                RAGE.NUI.UIResText.Alignment.Centered, true, true, 0);
        }

        private void DrawAtAim()
        {
            int targetEntity = 0;
            if (!RAGE.Game.Player.GetEntityPlayerIsFreeAimingAt(ref targetEntity))
                return;

            if (Entity.GetEntityType(targetEntity) != (int)EntityTypeInGetEntityType.Ped)
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

                DrawNametag(nametag.Player.Handle, _utilsHandler.GetDisplayName(nametag.Player as ITDSPlayer), nametag.Distance);
            }
        }

        private void DrawSpectatedNametag()
        {
            if (!(_camerasHandler.Spectating.SpectatingEntity is ITDSPlayer target))
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
