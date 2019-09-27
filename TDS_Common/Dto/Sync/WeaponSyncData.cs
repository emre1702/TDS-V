using System.Collections.Generic;

namespace TDS_Common.Dto.Sync
{
    public class WeaponSyncData
    {
        public uint WeaponHash { get; set; }
        public int TintIndex { get; set; }
        public List<uint> ComponentHashes { get; set; }
    }
}
