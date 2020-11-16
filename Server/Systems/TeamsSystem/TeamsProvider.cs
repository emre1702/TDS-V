using Microsoft.Extensions.DependencyInjection;
using System;
using TDS.Server.Data.Interfaces.TeamsSystem;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Handler.Helper;

namespace TDS.Server.TeamsSystem
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
