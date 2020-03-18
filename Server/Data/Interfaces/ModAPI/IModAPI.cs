using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.Chat;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Server.Data.Interfaces.ModAPI.Marker;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Sync;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Server.Data.Interfaces.ModAPI.Thread;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Server.Data.Interfaces.ModAPI
{
    public interface IModAPI
    {
        IBlipAPI Blip { get; }
        IChatAPI Chat { get; }
        IColShapeAPI ColShape { get; }
        IMapObjectAPI MapObject { get; }
        IMarkerAPI Marker { get; }
        IPlayerAPI Player { get; }
        ISyncAPI Sync { get; }
        ITextLabelAPI TextLabel { get; }
        IThreadAPI Thread { get; }
        IVehicleAPI Vehicle { get; }
    }
}
