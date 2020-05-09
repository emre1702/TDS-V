using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.RAGEAPI.Extensions
{
    internal static class DecorationExtension
    {
        internal static GTANetworkAPI.Decoration ToMod(this Data.Models.Decoration decoration) 
            => new GTANetworkAPI.Decoration { Collection = decoration.Collection, Overlay = decoration.Overlay };

        internal static Data.Models.Decoration ToTDS(this GTANetworkAPI.Decoration decoration)
            => new Data.Models.Decoration { Collection = decoration.Collection, Overlay = decoration.Overlay };
    }
}
