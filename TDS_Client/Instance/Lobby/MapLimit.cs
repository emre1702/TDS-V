using RAGE;
using RAGE.Elements;
using RAGE.Game;
using RAGE.NUI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Instance.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Instance.Lobby
{
    class MapLimit
    {
        private float minX, minY, maxX, maxY;
        private List<Vector3> edges;
        private int maxOutsideCounter;

        private int outsideCounter;
        private DxText info;
        private TDSTimer checkTimer;

        public MapLimit(List<Vector3> edges)
        {
            if (edges.Count == 0)
                return;
            this.edges = edges;
            minX = edges.Min(v => v.X);
            minY = edges.Min(v => v.Y);
            maxX = edges.Max(v => v.X);
            maxY = edges.Max(v => v.Y);
        }

        public void Start()
        {
            maxOutsideCounter = (int) Settings.DieAfterOutsideMapLimitTime;
            checkTimer = new TDSTimer(Check, 1000, 0);
        }

        public void Stop()
        {
            checkTimer?.Kill();
        }

        public void Remove()
        {
            checkTimer?.Kill();
            checkTimer = null;
            info?.Remove();
            info = null;
            edges = null;
        }

        private void Check()
        {
            if (edges == null || IsWithin())
            {
                Reset();
                return;
            }
            --outsideCounter;
            if (outsideCounter > 0 && outsideCounter <= 10)
                RefreshInfo();
            else if (outsideCounter == 0)
            {
                Events.CallRemote(DToServerEvent.OutsideMapLimit);
                Reset();
                checkTimer?.Kill();
                checkTimer = null;
            }      
        }

        private void RefreshInfo()
        {
            if (info == null)
                info = new DxText(Settings.Language.OUTSIDE_MAP_LIMIT.Replace("{1}", outsideCounter.ToString()), 0.5f, 0.5f, 1.2f, Color.White, 
                    alignmentX: UIResText.Alignment.Centered, alignmentY: EAlignmentY.Center);
            else
                info.Text = Settings.Language.OUTSIDE_MAP_LIMIT.Replace("{1}", outsideCounter.ToString());
        }

        private bool IsWithin() => IsWithin(Player.LocalPlayer.Position);

        private bool IsWithin(Vector3 point)
        {
            if (point.X < minX || point.Y < minY || point.X > maxX || point.Y > maxY)
                return false;

            bool inside = false;
            for (int i = 0, j = edges.Count-1; i < edges.Count; j = i++)
            {
                Vector3 iPoint = edges[i];
                Vector3 jPoint = edges[j];
                bool intersect = ((iPoint.Y > point.Y) != (jPoint.Y > point.Y))
                        && (point.X < (jPoint.X - iPoint.X) * (point.Y - iPoint.Y) / (jPoint.Y - iPoint.Y) + iPoint.X);
                if (intersect)
                    inside = !inside;
            }
            return inside;
        }

        private void Reset()
        {
            if (outsideCounter == maxOutsideCounter)
                return;
            info?.Remove();
            info = null;
            outsideCounter = maxOutsideCounter;
        }  
    }
}
