using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;

namespace TDS_Server.Data.Interfaces.Entities
{
    #nullable enable
    public interface ITDSVehicle : IVehicle
    {
        List<ITDSPlayer> Occupants { get; }
        byte Seats { get; }
        bool HasFreeSeat { get; }
        new ITDSPlayer? Driver { get; }

        void Delete();
        void Freeze(bool toggle, ILobby forLobby);
        void SetInvincible(bool toggle, ILobby forLobby);
        void SetInvincible(bool toggle, ITDSPlayer forPlayer);
    }
}
