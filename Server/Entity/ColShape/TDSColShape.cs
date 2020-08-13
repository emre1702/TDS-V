using System;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.ColShape
{
    public class TDSColShape : AltV.Net.Elements.Entities.ColShape, ITDSColShape
    {
        public TDSColShape(IntPtr nativePointer) : base(nativePointer)
        {

        }
    }
}
