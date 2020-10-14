using Microsoft.Extensions.DependencyInjection;
using System;
using TDS_Server.Data.Interfaces.TeamsSystem;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Handler.Helper;

namespace TDS_Server.TeamsSystem
{
    public class TeamsProvider : ITeamsProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public TeamsProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITeam Create(Teams entity)
        {
            var team = _serviceProvider.GetRequiredService<ITeam>();
            team.Init(entity);
            return team;
        }
    }
}
