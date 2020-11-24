using System.Threading.Tasks;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangsSystem;

namespace TDS.Server.Data.Interfaces.GangActionAreaSystem.Action
{
#nullable enable
    public interface IBaseGangActionAreaAction
    {
        void Init(IBaseGangActionArea area);

        ValueTask Attack(IGang attacker);
    }
}