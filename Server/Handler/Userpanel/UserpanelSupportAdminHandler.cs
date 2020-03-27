﻿using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Userpanel
{
    class UserpanelSupportAdminHandler
    {
        private readonly UserpanelSupportRequestHandler _userpanelSupportRequestHandler;

        public UserpanelSupportAdminHandler(UserpanelSupportRequestHandler userpanelSupportRequestHandler)
            => _userpanelSupportRequestHandler = userpanelSupportRequestHandler;

        public Task<string?> GetData(ITDSPlayer player)
        {
            if (player.Entity is null)
                return Task.FromResult((string?)null);

            return _userpanelSupportRequestHandler.GetSupportRequests(player);
        }
    }
}