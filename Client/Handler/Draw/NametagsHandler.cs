using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Extensions;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;
using TDS_Client.Handler;

namespace TDS_Client.Handler.Draw
{
    public class NametagsHandler
    {
        private readonly IModAPI _modAPI;
        private readonly CamerasHandler _camerasHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly UtilsHandler _utilsHandler;

        public NametagsHandler(IModAPI modAPI, CamerasHandler camerasHandler, SettingsHandler settingsHandler, UtilsHandler utilsHandler)
        {
            _modAPI = modAPI;
            _camerasHandler = camerasHandler;
            _settingsHandler = settingsHandler;
            _utilsHandler = utilsHandler;

            modAPI.Event.TickNametag.Add(new EventMethodData<TickNametagDelegate>(Draw));
        }

        public void Draw(List<TickNametagData> nametags)
        {
            if (_settingsHandler.ShowNametagOnlyOnAiming)
                DrawAtAim();
            else
                DrawAtSight(nametags);
        }

        private void DrawAtAim()
        {
            int targetEntity = 0;
            if (!_modAPI.Player.GetEntityPlayerIsFreeAimingAt(ref targetEntity))
                return;

            if (_modAPI.Entity.GetEntityType(targetEntity) != EntityType.Ped)
                return;

            var myPos = _camerasHandler.ActiveCamera?.Position ?? _modAPI.LocalPlayer.Position;
            var hisPos = _modAPI.Entity.GetEntityCoords(targetEntity, true);
            var distance = myPos.DistanceTo(hisPos);

            if (distance > _settingsHandler.NametagMaxDistance)
                return;

            string name = "Ped";
            var player = _modAPI.Pool.Players.GetAtHandle(targetEntity);
            if (!(player is null))
                name = _utilsHandler.GetDisplayName(player);

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

                DrawNametag(nametag.Player.Handle, _utilsHandler.GetDisplayName(nametag.Player), nametag.Distance);
            }
        }

        public void DrawNametag(int handle, string name, float distance)
        {
            float scale = Math.Max(distance / _settingsHandler.NametagMaxDistance, 0.5f);
            var position = _modAPI.Entity.GetEntityCoords(handle, true);
            position.Z += 0.9f + distance / _settingsHandler.NametagMaxDistance;

            float screenX = 0;
            float screenY = 0;
            _modAPI.Graphics.GetScreenCoordFromWorldCoord(position.X, position.Y, position.Z, ref screenX, ref screenY);

            float textheight = _modAPI.Ui.GetTextScaleHeight(scale, Font.ChaletLondon);
            screenY -= textheight;

            _modAPI.Graphics.DrawText(name, (int)(1920 * screenX), (int)(1080 * screenY), Font.ChaletLondon, scale, GetHealthColor(handle),
                AlignmentX.Center, true, true, 0);
        }


        private Color GetHealthColor(int handle)
        {
            var hp = Math.Max(_modAPI.Entity.GetEntityHealth(handle) - 100, 0);
            var armor = _modAPI.Ped.GetPedArmor(handle);

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
    }
}
