using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.TeamsSystem;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
#nullable enable

    public interface IPlayerTeamHandler
    {
        ITeam? Team { get; }

        void Init(ITDSPlayer player);

        void SetTeam(ITeam? team, bool forceIsNew);
    }
}
