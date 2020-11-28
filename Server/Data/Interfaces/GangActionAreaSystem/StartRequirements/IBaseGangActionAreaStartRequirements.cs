using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;

namespace TDS.Server.Data.Interfaces.GangActionAreaSystem.StartRequirements
{
#nullable enable
    public interface IBaseGangActionAreaStartRequirements
    {
        bool HasCooldown { get; set; }
        void Init(IBaseGangActionArea area);
    }
}