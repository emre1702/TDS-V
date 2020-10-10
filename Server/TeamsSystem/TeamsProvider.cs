using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Helper;

namespace TDS_Server.TeamsSystem
{
    public class TeamsProvider : ITeamsProvider
    {
        private readonly LangHelper _langHelper;

        public TeamsProvider(LangHelper langHelper)
        {
            _langHelper = langHelper;
        }

        public ITeam Create(Teams entity)
            => new Team(entity, _langHelper);
    }
}
