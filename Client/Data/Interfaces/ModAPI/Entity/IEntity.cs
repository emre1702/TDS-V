using System;
using System.Collections.Generic;
using System.Text;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Entity
{
    public interface IEntity
    {
        uint Handle { get; }
        Position3D Position { get; set; }
        Position3D Rotation { get; set; }

    }
}
