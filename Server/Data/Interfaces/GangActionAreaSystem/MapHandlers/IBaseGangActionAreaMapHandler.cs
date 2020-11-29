using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Models.Map;

namespace TDS.Server.Data.Interfaces.GangActionAreaSystem.MapHandlers
{
    public interface IBaseGangActionAreaMapHandler
    {
        MapDto Map { get; }

        void Init(IBaseGangActionArea area, MapDto map);
    }
}