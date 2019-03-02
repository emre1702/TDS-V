using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RAGE.Elements;
using RAGE.Game;
using System.Collections.Generic;
using System.Linq;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Utility
{
    static class ClientUtils
    {
        public static List<Player> GetTriggeredPlayersList(object argobj)
        {
            List<ushort> arg = ((JArray)argobj).ToObject<List<ushort>>();
            return arg.Select(s => Entities.Players.GetAtRemote(s)).ToList();
        }

        public static void DisableAttack()
        {
            Pad.DisableControlAction(1, 24, true);
            Pad.DisableControlAction(1, 140, true);
            Pad.DisableControlAction(1, 141, true);
            Pad.DisableControlAction(1, 142, true);
            Pad.DisableControlAction(1, 257, true);
            Pad.DisableControlAction(1, 263, true);
            Pad.DisableControlAction(1, 264, true);
        }

        public static void Notify(string msg)
        {
            Ui.SetNotificationTextEntry("STRING");
            Ui.AddTextComponentSubstringPlayerName(msg);
            Ui.DrawNotification(false, false);
        }
    }
}
