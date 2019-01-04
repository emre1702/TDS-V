using RAGE.Game;
using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;

namespace TDS_Client.Manager.Lobby
{
    static class MapInfo
    {
        public static string CurrentMap => mapInfo?.Text ?? "-";

        private static DxText mapInfo;

        public static void SetMapInfo(string mapname)
        {
            //currentMap = mapname;
            if (mapInfo == null)
                mapInfo = new DxText(mapname, 0.01f, 0.99f, 0.2f, Color.White, alignmentX: RAGE.NUI.UIResText.Alignment.Left, alignmentY: EAlignmentY.Bottom);
            else
                mapInfo.Text = mapname;
        }

        public static void RemoveMapInfo()
        {
            mapInfo?.Remove();
            mapInfo = null;
        }
    }
}
