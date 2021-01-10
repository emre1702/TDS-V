using System;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Models
{
    public class RemoteBrowserEventArgs
    {
        public ITDSPlayer Player { get; set; }
        public string EventName { get; set; }
#pragma warning disable CA1051 // Do not declare visible instance fields
        public ArraySegment<object> Args;
#pragma warning restore CA1051 // Do not declare visible instance fields

        public RemoteBrowserEventArgs(ITDSPlayer player, params object[] args)
        {
            Player = player;
            EventName = (string)args[0];
            Args = new ArraySegment<object>(args, 1, args.Length - 1);
        }
    }
}