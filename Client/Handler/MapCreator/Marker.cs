using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class Marker
    {
        public MarkerType Type;
        public Position3D Position;
        public Position3D Direction;
        public Position3D Rotation;
        public Position3D Scale;
        public Color Color;

        private readonly IModAPI _modAPI;

        public Marker(IModAPI modAPI, MarkerType type, Position3D pos, Position3D dir, Position3D rot, Position3D scale, Color col)
        {
            _modAPI = modAPI;

            Type = type;
            Position = pos;
            Direction = dir;
            Rotation = rot;
            Color = col;
            Scale = scale;
        }

        public void Draw()
        {
            _modAPI.Graphics.DrawMarker(Type, Position.X, Position.Y, Position.Z,
                Direction.X, Direction.Y, Direction.Z, Rotation.X, Rotation.Y, Rotation.Z, Scale.X, Scale.Y, Scale.Z,
                Color.R, Color.G, Color.B, Color.A,
                false, false, false, "", "", false);
        }
    }
}
