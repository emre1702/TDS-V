using RAGE;
using System.Drawing;
using TDS.Client.Data.Enums;

namespace TDS.Client.Handler.MapCreator
{
    public class Marker
    {
        public Color Color;
        public Vector3 Direction;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public MarkerType Type;

        public Marker(MarkerType type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, Color col)
        {
            Type = type;
            Position = pos;
            Direction = dir;
            Rotation = rot;
            Color = col;
            Scale = scale;
        }

        public void Draw()
        {
            /*_RAGE.Game.Graphics.DrawMarker(Type, Position.X, Position.Y, Position.Z,
                Direction.X, Direction.Y, Direction.Z, Rotation.X, Rotation.Y, Rotation.Z, Scale.X, Scale.Y, Scale.Z,
                Color.R, Color.G, Color.B, Color.A,
                false, false, false, "", "", false);*/
            RAGE.Events.CallLocal("drawMarker", Type, Position.X, Position.Y, Position.Z,
                Direction.X, Direction.Y, Direction.Z, Rotation.X, Rotation.Y, Rotation.Z, Scale.X, Scale.Y, Scale.Z,
                Color.R, Color.G, Color.B, Color.A,
                false, false, 2, false, "", "", false);
        }
    }
}
