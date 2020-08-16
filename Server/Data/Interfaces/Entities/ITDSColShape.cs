using AltV.Net.Elements.Entities;
using System;

namespace TDS_Server.Data.Interfaces.Entities
{
    #nullable enable
    public interface ITDSColShape : IColShape
    {
        Action<ITDSPlayer>? PlayerEntered { get; set; }
        Action<ITDSPlayer>? PlayerExited { get; set; }

        void Delete();
        void OnPlayerEntered(ITDSPlayer player);
        void OnPlayerExited(ITDSPlayer player);
    }
}
