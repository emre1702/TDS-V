using System;

namespace TDS_Server.Data.Interfaces.ModAPI.ColShape
{
    #nullable enable
    public interface IColShape : IEquatable<IColShape>
    {
        ushort Id { get; }

        public delegate void ColShapeEnterExitDelegate(ITDSPlayer player);
        public event ColShapeEnterExitDelegate? PlayerEntered;
        public event ColShapeEnterExitDelegate? PlayerExited;

        void Delete();
    }
}
