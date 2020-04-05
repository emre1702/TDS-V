using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Cam
{
    public interface ICam
    {
        Position3D Position { get; set; }
        Position3D Rotation { get; set; }

        void Render(bool render, bool ease, int easeTime);
        void AttachTo(IPedBase ped, PedBone bone, int x, float y, float z, bool heading);

        void Detach();

        void Destroy();
        void PointAtCoord(Position3D pos);
        void SetActive(bool active);
    }
}
