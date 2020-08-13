using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.MapCreator
{
    public class Marker
    {
        #region Public Fields

        public Color Color;
        public Position Direction;
        public Position Position;
        public Position Rotation;
        public Position Scale;
        public MarkerType Type;

        #endregion Public Fields

        #region Private Fields

        private readonly IModAPI _modAPI;

        #endregion Private Fields

        #region Public Constructors

        public Marker(IModAPI modAPI, MarkerType type, Position pos, Position dir, Position rot, Position scale, Color col)
        {
            _modAPI = modAPI;

            Type = type;
            Position = pos;
            Direction = dir;
            Rotation = rot;
            Color = col;
            Scale = scale;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Draw()
        {
            /*_modAPI.Graphics.DrawMarker(Type, Position.X, Position.Y, Position.Z,
                Direction.X, Direction.Y, Direction.Z, Rotation.X, Rotation.Y, Rotation.Z, Scale.X, Scale.Y, Scale.Z,
                Color.R, Color.G, Color.B, Color.A,
                false, false, false, "", "", false);*/
            _modAPI.Event.CallLocal("drawMarker", Type, Position.X, Position.Y, Position.Z,
                Direction.X, Direction.Y, Direction.Z, Rotation.X, Rotation.Y, Rotation.Z, Scale.X, Scale.Y, Scale.Z,
                Color.R, Color.G, Color.B, Color.A,
                false, false, 2, false, "", "", false);
        }

        #endregion Public Methods
    }
}
