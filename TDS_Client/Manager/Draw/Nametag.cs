using Player = RAGE.Elements.Player;
using System.Collections.Generic;
using TDS_Client.Manager.Utility;
using static RAGE.Events;
using System.Drawing;
using System;
using Font = RAGE.Game.Font;
using RAGE.Elements;
using TDS_Client.Enum;
using RAGE.Game;

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

            PedBase target = Entities.Players.GetAtHandle(targetEntity);
            if (target == null)
            {
                target = Entities.Peds.GetAtHandle(targetEntity);
                if (target == null)
                    return;
            }

            var distance = target.Position.DistanceTo(Player.LocalPlayer.Position);
            if (target.Position.DistanceTo(Player.LocalPlayer.Position) > Settings.NametagMaxDistance)
                return;

            DrawNametag(target, distance);
        }

        private static void DrawAtSight(List<TickNametagData> nametags)
        {
            if (nametags == null)
                return;
            foreach (var nametag in nametags)
            {
                if (nametag.Distance > Settings.NametagMaxDistance)
                    continue;

                DrawNametag(nametag.Player, nametag.Distance);
            }
        }

        private static void DrawNametag(PedBase target, float distance)
        {
            float scale = Math.Max(distance / Settings.NametagMaxDistance, 0.6f);
            var position = RAGE.Game.Ped.GetPedBoneCoords(target.Handle, (int)EPedBone.IK_Head, 0, 0, 0.9f);

            float screenX = 0;
            float screenY = 0;
            Graphics.GetScreenCoordFromWorldCoord(position.X, position.Y, position.Z, ref screenX, ref screenY);

            float textheight = Ui.GetTextScaleHeight(scale, (int)Font.ChaletLondon);
            screenY -= textheight;

            RAGE.Chat.Output(screenX + " - " + screenY + " | " + scale);

            RAGE.NUI.UIResText.Draw(target is Player ? ((Player)target).Name : "Ped", (int)(1920 * screenX), (int)(1080 * screenY), Font.ChaletLondon, scale, GetHealthColor(target),
                RAGE.NUI.UIResText.Alignment.Centered, true, true, 0);
        }


        private static Color GetHealthColor(PedBase pedBase)
        {
            var hp = Math.Max(pedBase.GetHealth() - 100, 0);
            var armor = pedBase.GetArmour();
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
