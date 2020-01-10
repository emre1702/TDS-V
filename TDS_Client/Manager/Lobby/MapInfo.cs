using System.Drawing;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;

namespace TDS_Client.Manager.Lobby
{
    internal static class MapInfo
    {
        public static string CurrentMap => _mapInfo?.Text ?? "-";

        private static DxText _mapInfo;

        public static void SetMapInfo(string mapname)
        {
            //currentMap = mapname;
            if (_mapInfo == null)
                _mapInfo = new DxText(mapname, 0.01f, 0.995f, 0.2f, Color.White, alignmentX: RAGE.NUI.UIResText.Alignment.Left, alignmentY: EAlignmentY.Bottom);
            else
                _mapInfo.Text = mapname;
        }

        public static void RemoveMapInfo()
        {
            _mapInfo?.Remove();
            _mapInfo = null;
        }
    }
}
