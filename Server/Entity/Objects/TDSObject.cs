using AltV.Net.Data;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.Objects
{
    //Todo: Implement this
    public class TDSObject : ITDSObject
    {

        public TDSObject(int model, Position position, DegreeRotation rotation, byte alpha, int dimension)
        {
            Position = position;
            Rotation = rotation;
        }

        public Position Position { get; set; }
        public DegreeRotation Rotation { get; set; }

        public void AttachTo(ITDSPlayer player, PedBone bone, Position? offsetPos, DegreeRotation? offsetRot)
        {
            //Todo: Implement this
        }

        public void Delete()
        {
            //Todo: Implement this
        }

        public void Detach()
        {
            //Todo: Implement this
        }

        public void Freeze(bool toggle, ILobby forLobby)
        {
            //Todo: Implement this
        }

        public void SetCollisionsless(bool toggle, ILobby forLobby)
        {
            //Todo: Implement this
        }
    }
}
