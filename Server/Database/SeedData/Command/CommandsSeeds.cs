using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Command;

namespace TDS.Server.Database.SeedData.Command
{
    public static class CommandsSeeds
    {
        public static ModelBuilder HasCommands(this ModelBuilder modelBuilder)
        {
            var seedCommands = new List<Commands>
            {
                new Commands { Id = 1, Command = "AdminSay", NeededAdminLevel = 1 },
                new Commands { Id = 2, Command = "AdminChat", NeededAdminLevel = 1, VipCanUse = true },
                new Commands { Id = 3, Command = "Ban", NeededAdminLevel = 2 },
                new Commands { Id = 4, Command = "Goto", NeededAdminLevel = 2, LobbyOwnerCanUse = true },
                new Commands { Id = 5, Command = "Kick", NeededAdminLevel = 1, VipCanUse = true },
                new Commands { Id = 6, Command = "LobbyBan", NeededAdminLevel = 1, VipCanUse = true, LobbyOwnerCanUse = true },
                new Commands { Id = 7, Command = "LobbyKick", NeededAdminLevel = 1, VipCanUse = true, LobbyOwnerCanUse = true },
                new Commands { Id = 8, Command = "Mute", NeededAdminLevel = 1, VipCanUse = true },
                new Commands { Id = 9, Command = "NextMap", NeededAdminLevel = 1, VipCanUse = true, LobbyOwnerCanUse = true },
                new Commands { Id = 10, Command = "LobbyLeave" },
                new Commands { Id = 11, Command = "Suicide" },
                new Commands { Id = 12, Command = "GlobalChat" },
                new Commands { Id = 13, Command = "TeamChat" },
                new Commands { Id = 14, Command = "PrivateChat" },
                new Commands { Id = 15, Command = "Position" },
                new Commands { Id = 16, Command = "ClosePrivateChat" },
                new Commands { Id = 17, Command = "OpenPrivateChat" },
                new Commands { Id = 18, Command = "PrivateMessage" },
                new Commands { Id = 19, Command = "UserId" },
                new Commands { Id = 20, Command = "BlockUser" },
                new Commands { Id = 21, Command = "UnblockUser" },
                new Commands { Id = 22, Command = "LoadMapOfOthers", NeededAdminLevel = 1, VipCanUse = true },    // not a command
                new Commands { Id = 23, Command = "VoiceMute", NeededAdminLevel = 1, VipCanUse = true },
                new Commands { Id = 24, Command = "GiveMoney" },
                new Commands { Id = 25, Command = "LobbyInvitePlayer", LobbyOwnerCanUse = true },
                new Commands { Id = 26, Command = "Test", NeededAdminLevel = 3 },
                new Commands { Id = 27, Command = "CreateHouse", NeededAdminLevel = 2 },
                new Commands { Id = 28, Command = "Admins" },
                new Commands { Id = 29, Command = "SetAdmin", NeededAdminLevel = 3 },
                new Commands { Id = 30, Command = "SetAdminLeader", NeededAdminLevel = 3 },
                new Commands { Id = 31, Command = "SetVip", NeededAdminLevel = 3 },
            };
            modelBuilder.Entity<Commands>().HasData(seedCommands);
            return modelBuilder;
        }
    }
}
