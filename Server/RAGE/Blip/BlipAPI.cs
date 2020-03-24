using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGE.Blip
{
    class BlipAPI : IBlipAPI
    {
        public IBlip Create(Position3D position, ILobby lobby)
        {
            throw new NotImplementedException();
        }
    }
}
