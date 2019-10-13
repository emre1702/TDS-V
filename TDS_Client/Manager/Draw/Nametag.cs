using Player = RAGE.Elements.Player;
using PedBase = RAGE.Elements.PedBase;
using System.Collections.Generic;
using TDS_Client.Manager.Utility;
using static RAGE.Events;
using System.Drawing;
using System;
using Font = RAGE.Game.Font;
using TDS_Client.Enum;
using RAGE.Game;
using TDS_Client.Instance.Utility;

namespace TDS_Client.Manager.Draw
{
    class Nametag
    {

        public static void Draw(List<TickNametagData> nametags) 
        {
            if (Settings.ShowNametagOnlyOnAiming)
                DrawAtAim();
            else
                DrawAtSight(nametags);
        }

        private static void DrawAtAim()
        {
            int targetEntity = 0;
            if (!RAGE.Game.Player.GetEntityPlayerIsFreeAimingAt(ref targetEntity))
                return;

            if (Entity.GetEntityType(targetEntity) != 1)
                return;

            var myPos = TDSCamera.ActiveCamera?.Position ?? Player.LocalPlayer.Position;
            var hisPos = Entity.GetEntityCoords(targetEntity, true);
            var distance = myPos.DistanceTo(hisPos);

            if (distance > Settings.NametagMaxDistance)
                return;

            string name = "Ped";
            RAGE.Elements.Entities.Players.All.ForEach(player => 
            {
                if (player.Handle == targetEntity)
                    name = player.Name;
            });

            DrawNametag(targetEntity, name, distance);
        }

        private static void DrawAtSight(List<TickNametagData> nametags)
        {
            if (nametags == null)
                return;
            foreach (var nametag in nametags)
            {
                if (nametag.Distance > Settings.NametagMaxDistance)
                    continue;

                DrawNametag(nametag.Player.Handle, nametag.Player.Name, nametag.Distance);
            }
        }

        public static void DrawNametag(int handle, string name, float distance)
        {
            float scale = Math.Max(distance / Settings.NametagMaxDistance, 0.6f);
            var position = Entity.GetEntityCoords(handle, true);
            position.Z += 0.9f + distance / Settings.NametagMaxDistance;

            float screenX = 0;
            float screenY = 0;
            Graphics.GetScreenCoordFromWorldCoord(position.X, position.Y, position.Z, ref screenX, ref screenY);

            float textheight = Ui.GetTextScaleHeight(scale, (int)Font.ChaletLondon);
            screenY -= textheight;

            RAGE.NUI.UIResText.Draw(name, (int)(1920 * screenX), (int)(1080 * screenY), Font.ChaletLondon, scale, GetHealthColor(handle),
                RAGE.NUI.UIResText.Alignment.Centered, true, true, 0);
        }


        private static Color GetHealthColor(int handle)
        {
            var hp = Math.Max(Entity.GetEntityHealth(handle) - 100, 0);
            var armor = Ped.GetPedArmour(handle);
            return GetHealthColor(hp, armor);
        }

        private static Color GetHealthColor(int hp, int armor)
        {
            if (hp == 0)
                return Color.FromArgb(ClientConstants.NametagAlpha, 0, 0, 0);

            if (armor == 0)
                return Color.FromArgb(ClientConstants.NametagAlpha, (int)Math.Ceiling((100 - hp) * 2.55 / 2), (int)Math.Ceiling(hp * 2.55), 0);

            if (armor > 100)
                return Color.FromArgb(ClientConstants.NametagAlpha,
                    (int)Math.Ceiling(255 - hp * 2.55),
                    (int)Math.Ceiling(255 - hp * 2.55 / 2 - (armor - 100) * 2.55 / 2),
                    (int)Math.Ceiling(255 - hp * 2.55 / 2));

            return Color.FromArgb(ClientConstants.NametagAlpha,
                    (int)Math.Ceiling(armor * 2.55),
                    (int)Math.Ceiling(armor * 2.55 / 2 + hp * 2.55 / 2),
                    (int)Math.Ceiling(armor * 2.55));

        }
    }
}
