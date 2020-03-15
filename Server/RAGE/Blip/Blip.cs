using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Models.GTA;
using TDS_Server.RAGE.Extensions;

namespace TDS_Server.RAGE.Blip
{
    class Blip : IBlip
    {
        internal readonly GTANetworkAPI.Blip _instance;

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

        public Blip(Position3D position, uint dimension = uint.MaxValue)
        {
            _instance = NAPI.Blip.CreateBlip(position.ToVector3(), dimension);
        }

        public Blip(uint sprite, Position3D position, float scale, byte color, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = false, short rotation = 0, uint dimension = uint.MaxValue)
        {
            _instance = NAPI.Blip.CreateBlip(sprite, position.ToVector3(), scale, color, name, alpha, drawDistance, shortRange, rotation, dimension);
        }

        public Blip(int sprite, Position3D position, float scale, byte color, string name = "", byte alpha = 255, float drawDistance = 0, bool shortRange = false, short rotation = 0, uint dimension = uint.MaxValue)
            : this((uint)sprite, position, scale, color, name, alpha, drawDistance, shortRange, rotation, dimension) { }

        public Blip(Position3D position, float range, uint dimension = uint.MaxValue)
        {
            _instance = NAPI.Blip.CreateBlip(position.ToVector3(), range, dimension);
        }

        public void Delete()
        {
            _instance.Delete();
        }
    }
}
