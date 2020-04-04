using System;
using System.Collections.Generic;
using System.Text;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Cam
{
    public interface ICam
    {
        Position3D Position { get; set; }
        Position3D Rotation { get; set; }

        void Render(bool render, bool ease, int easeTime);
        void Detach();





        void Destroy();
    }
}
