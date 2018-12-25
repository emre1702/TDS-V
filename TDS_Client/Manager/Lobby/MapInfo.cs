using RAGE.Game;
using System.Drawing;
using TDS_Client.Instance.Draw.Dx;

namespace TDS_Client.Manager.Lobby
{
    static class MapInfo
    {
        private static DxText mapInfo;

        public static void SetMapInfo(string mapname)
        {
            //currentMap = mapname;
            if (mapInfo == null)
                mapInfo = new DxText(mapname, 0, 0.95f, 0.5f, Color.White, alignment: Alignment.Center);
            else
                mapInfo.SetText(mapname);
        }

        public static void RemoveMapInfo()
        {
            mapInfo?.Remove();
            mapInfo = null;
        }
    }
}
