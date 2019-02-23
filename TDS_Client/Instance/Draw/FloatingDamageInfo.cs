using RAGE;
using RAGE.Elements;
using RAGE.Game;
using RAGE.NUI;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Instance.Draw
{
    class FloatingDamageInfo
    {
        private static List<FloatingDamageInfo> damageInfos = new List<FloatingDamageInfo>();

        private float damage;
        private ulong startTicks;
        private Vector3 targetPosition;
        private DxText text;
        private bool remove = false;

        public static void Create(Player target, float damage)
        {
            FloatingDamageInfo damageInfo = new FloatingDamageInfo()
            {
                damage = damage,
                startTicks = TimerManager.ElapsedTicks,
                targetPosition = target.Position
            };
            damageInfos.Add(damageInfo);
        } 

        ~FloatingDamageInfo() {
            text.Remove();
        }
        
        private void UpdatePosition()
        {
            ulong elapsedTicks = TimerManager.ElapsedTicks - startTicks;
            if (elapsedTicks > Constants.ShowFloatingDamageInfoMs)
            {
                remove = true;
                return;
            }
            float screenX = 0;
            float screenY = 0;
            Graphics.GetScreenCoordFromWorldCoord(targetPosition.X, targetPosition.Y, targetPosition.Z + 1, ref screenX, ref screenY);

            float percentage = elapsedTicks / Constants.ShowFloatingDamageInfoMs;
            screenY += percentage * 5;

            if (text == null)
                text = new DxText(damage.ToString(), screenX, screenY, 0.4f, Color.White, alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Bottom, dropShadow: true, outline: true);
            else
                text.SetRelativeY(screenY);
        }

        public static void UpdateAllPositions()
        {
            if (damageInfos.Count == 0)
                return;
            damageInfos.RemoveAll(x => x.remove);
            foreach (var damageInfo in damageInfos)
            {
                damageInfo.UpdatePosition();
            }
        }
    }
}
