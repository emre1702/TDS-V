using RAGE;

namespace TDS_Client.Instance.MapCreator
{
    class Marker
    {
        public int Type;
        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 Rotation;
        public Vector3 Scale;
        public RGBA Color;

        public Marker(int type, Vector3 pos, Vector3 dir, Vector3 rot, Vector3 scale, RGBA col)
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
            RAGE.Events.CallLocal("drawMarker", Type, Position.X, Position.Y, Position.Z,
                Direction.X, Direction.Y, Direction.Z, Rotation.X, Rotation.Y, Rotation.Z, Scale.X, Scale.Y, Scale.Z,
                Color.Red, Color.Green, Color.Blue, Color.Alpha,
                false, false, 2, false, "", "", false);
        }
    }
}
