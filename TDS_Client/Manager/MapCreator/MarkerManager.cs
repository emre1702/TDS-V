using RAGE;
using RAGE.Game;
using System.Linq;
using TDS_Client.Instance.MapCreator;

namespace TDS_Client.Manager.MapCreator
{
    class MarkerManager
    {
        private static AxisMarker[] _rotateMarker;
        private static AxisMarker _clickedMarker;

        public static void Start()
        {
            _rotateMarker = new AxisMarker[]
            {
                new AxisMarker(new RGBA(255, 0, 0), rot => new Vector3(-rot + 90f, 90, 0)),     // Marker_X_Rotate
                new AxisMarker(new RGBA(0, 255, 0), rot => new Vector3(-rot, 90, 0)),           // Marker_Y_Rotate
                new AxisMarker(new RGBA(0, 0, 255), rot => new Vector3(0, 0, 0)),               // Marker_Z_Rotate
                new AxisMarker(new RGBA(255, 0, 0)),    // Marker_X_Move
                new AxisMarker(new RGBA(0, 255, 0)),    // Marker_Y_Move
                new AxisMarker(new RGBA(0, 0, 255)),    // Marker_Z_Move
            };
            _clickedMarker = null;
        }

        public static void Stop()
        {
            _rotateMarker = null;
            _clickedMarker = null;
        }

        public static void OnTick()
        {
            if (_rotateMarker == null)
                return;

            if (ObjectPlacing.HighlightedObject == null)
                return;

            if (_clickedMarker != null)
            {
                if (Pad.IsDisabledControlJustReleased(0, (int)Control.Attack))
                {
                    _clickedMarker = null;
                    ObjectPlacing.HighlightedObject.LoadEntityData();
                }
                else
                {
                    _clickedMarker.HandleClick(ObjectPlacing.HighlightedObject);
                }
            }
        }

    }
}
