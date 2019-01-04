using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RAGE;
using RAGE.Elements;
using System.Collections.Generic;
using System.Linq;

namespace TDS_Client.Manager.Utility
{
    static class ClientUtils
    {
        public static List<Player> GetTriggeredPlayersList(object argobj)
        {
            List<ushort> arg = ((JArray)argobj).ToObject<List<ushort>>();
            return arg.Select(s => Entities.Players.GetAtRemote(s)).ToList();
        }
    }
}
