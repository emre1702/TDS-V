using AltV.Net.Data;
using System;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;

namespace TDS_Server.Entity.Vehicle
{
    //Todo: Implement these
    public class TDSVehicle : AltV.Net.Elements.Entities.Vehicle, ITDSVehicle
    {

        public TDSVehicle(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {

        }

        public TDSVehicle(uint model, Position position, Rotation rotation): base(model, position, rotation) { }

        public List<ITDSPlayer> Occupants => throw new NotImplementedException();

        public byte Seats => 0;

        public bool HasFreeSeat => false;

        ITDSPlayer? ITDSVehicle.Driver => base.Driver as ITDSPlayer;

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Freeze(bool toggle, ILobby forLobby)
        {
            throw new NotImplementedException();
        }

        public void SetInvincible(bool toggle, ILobby forLobby)
        {
            throw new NotImplementedException();
        }

        public void SetInvincible(bool toggle, ITDSPlayer forPlayer)
        {
            throw new NotImplementedException();
        }
    }
}
