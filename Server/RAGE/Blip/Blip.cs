using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.RAGE.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGE.Blip
{
    class Blip : IBlip
    {
        internal readonly GTANetworkAPI.Blip _instance;

        public Blip(GTANetworkAPI.Blip instance)
        {
            _instance = instance;
        }

        public ushort Id => _instance.Id;
        public string Name
        {
            get => _instance.Name;
            set => _instance.Name = value;
        }
        public uint Sprite
        {
            get => _instance.Sprite;
            set => _instance.Sprite = value;
        }
        public int Color 
        { 
            get => _instance.Color;
            set => _instance.Color = value; 
        }

        public void Delete()
        {
            _instance.Delete();
        }

        public bool Equals(IBlip? other)
        {
            return _instance.Id == other?.Id;
        }
    }
}
