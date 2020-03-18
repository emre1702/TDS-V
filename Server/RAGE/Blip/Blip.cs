using GTANetworkAPI;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.RAGE.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGE.Blip
{
    class Blip : IBlip
    {
        internal readonly GTANetworkAPI.Blip _instance;

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
        public int Color { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
