using System;

namespace TDS_Server.Data.Interfaces.ModAPI.Blip
{
    public interface IBlip : IEquatable<IBlip>
    {
        string Name { get; set; }
        uint Sprite { get; set; }
        ushort Id { get; }
        int Color { get; set; }

        void Delete();
    }
}
