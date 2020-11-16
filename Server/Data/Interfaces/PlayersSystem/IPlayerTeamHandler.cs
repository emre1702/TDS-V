using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.TeamsSystem;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
#nullable enable

    public interface IPlayerTeamHandler
    {
        ITeam? Team { get; }

        void Init(ITDSPlayer player);

        void SetTeam(ITeam? team, bool forceIsNew);
    }
}
