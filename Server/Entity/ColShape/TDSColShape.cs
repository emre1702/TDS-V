using System;
using TDS_Server.Data.Interfaces.Entities;

namespace TDS_Server.Entity.ColShape
{
    public class TDSColShape : AltV.Net.Elements.Entities.ColShape, ITDSColShape
    {
        public TDSColShape(IntPtr nativePointer) : base(nativePointer)
        {
            
        }

        public Action<ITDSPlayer>? PlayerEntered { get; set; }
        public Action<ITDSPlayer>? PlayerExited { get; set; }

        public void Delete()
            => Remove();

        public void OnPlayerEntered(ITDSPlayer player)
        {
            PlayerEntered?.Invoke(player);
        }

        public void OnPlayerExited(ITDSPlayer player)
        {
            PlayerExited?.Invoke(player);
        }
    }
}
