using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;

namespace TDS.Server.Data.Interfaces.GangActionAreaSystem.Action
{
#nullable enable
    public interface IBaseGangActionAreaAction
    {
        void Init(IBaseGangActionArea area);

        ValueTask Attack(ITDSPlayer attackerPlayer);
    }
}