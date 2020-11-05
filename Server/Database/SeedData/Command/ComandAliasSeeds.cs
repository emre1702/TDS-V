﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server.Database.Entity.Command;

namespace TDS_Server.Database.SeedData.Command
{
    public static class ComandAliasSeeds
    {
        public static ModelBuilder HasCommandAliases(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommandAlias>().HasData(
                new CommandAlias { Alias = "Announce", Command = 1 },
                new CommandAlias { Alias = "Announcement", Command = 1 },
                new CommandAlias { Alias = "ASay", Command = 1 },
                new CommandAlias { Alias = "OChat", Command = 1 },
                new CommandAlias { Alias = "OSay", Command = 1 },
                new CommandAlias { Alias = "AChat", Command = 2 },
                new CommandAlias { Alias = "ChatAdmin", Command = 2 },
                new CommandAlias { Alias = "InternChat", Command = 2 },
                new CommandAlias { Alias = "WriteAdmin", Command = 2 },
                new CommandAlias { Alias = "InternalChat", Command = 2 },
                new CommandAlias { Alias = "PBan", Command = 3 },
                new CommandAlias { Alias = "Permaban", Command = 3 },
                new CommandAlias { Alias = "RBan", Command = 3 },
                new CommandAlias { Alias = "TBan", Command = 3 },
                new CommandAlias { Alias = "Timeban", Command = 3 },
                new CommandAlias { Alias = "UBan", Command = 3 },
                new CommandAlias { Alias = "UnBan", Command = 3 },
                new CommandAlias { Alias = "GotoPlayer", Command = 4 },
                new CommandAlias { Alias = "GotoXYZ", Command = 4 },
                new CommandAlias { Alias = "Warp", Command = 4 },
                new CommandAlias { Alias = "WarpTo", Command = 4 },
                new CommandAlias { Alias = "WarpToPlayer", Command = 4 },
                new CommandAlias { Alias = "XYZ", Command = 4 },
                new CommandAlias { Alias = "RKick", Command = 5 },
                new CommandAlias { Alias = "BanLobby", Command = 6 },
                new CommandAlias { Alias = "KickLobby", Command = 7 },
                new CommandAlias { Alias = "PermaMute", Command = 8 },
                new CommandAlias { Alias = "PMute", Command = 8 },
                new CommandAlias { Alias = "RMute", Command = 8 },
                new CommandAlias { Alias = "TimeMute", Command = 8 },
                new CommandAlias { Alias = "TMute", Command = 8 },
                new CommandAlias { Alias = "EndRound", Command = 9 },
                new CommandAlias { Alias = "Next", Command = 9 },
                new CommandAlias { Alias = "Skip", Command = 9 },
                new CommandAlias { Alias = "Back", Command = 10 },
                new CommandAlias { Alias = "Leave", Command = 10 },
                new CommandAlias { Alias = "LeaveLobby", Command = 10 },
                new CommandAlias { Alias = "Mainmenu", Command = 10 },
                new CommandAlias { Alias = "Dead", Command = 11 },
                new CommandAlias { Alias = "Death", Command = 11 },
                new CommandAlias { Alias = "Die", Command = 11 },
                new CommandAlias { Alias = "Kill", Command = 11 },
                new CommandAlias { Alias = "AllChat", Command = 12 },
                new CommandAlias { Alias = "AllSay", Command = 12 },
                new CommandAlias { Alias = "G", Command = 12 },
                new CommandAlias { Alias = "GChat", Command = 12 },
                new CommandAlias { Alias = "Global", Command = 12 },
                new CommandAlias { Alias = "GlobalSay", Command = 12 },
                new CommandAlias { Alias = "PublicChat", Command = 12 },
                new CommandAlias { Alias = "PublicSay", Command = 12 },
                new CommandAlias { Alias = "TChat", Command = 13 },
                new CommandAlias { Alias = "TeamSay", Command = 13 },
                new CommandAlias { Alias = "TSay", Command = 13 },
                new CommandAlias { Alias = "PChat", Command = 14 },
                new CommandAlias { Alias = "PrivateSay", Command = 14 },
                new CommandAlias { Alias = "Coord", Command = 15 },
                new CommandAlias { Alias = "Coordinate", Command = 15 },
                new CommandAlias { Alias = "Coordinates", Command = 15 },
                new CommandAlias { Alias = "CurrentPos", Command = 15 },
                new CommandAlias { Alias = "CurrentPosition", Command = 15 },
                new CommandAlias { Alias = "GetPos", Command = 15 },
                new CommandAlias { Alias = "GetPosition", Command = 15 },
                new CommandAlias { Alias = "Pos", Command = 15 },
                new CommandAlias { Alias = "MSG", Command = 18 },
                new CommandAlias { Alias = "PM", Command = 18 },
                new CommandAlias { Alias = "PSay", Command = 18 },
                new CommandAlias { Alias = "CPC", Command = 16 },
                new CommandAlias { Alias = "ClosePM", Command = 16 },
                new CommandAlias { Alias = "ClosePrivateSay", Command = 16 },
                new CommandAlias { Alias = "StopPrivateChat", Command = 16 },
                new CommandAlias { Alias = "StopPrivateSay", Command = 17 },
                new CommandAlias { Alias = "OpenPrivateSay", Command = 17 },
                new CommandAlias { Alias = "OpenPM", Command = 17 },
                new CommandAlias { Alias = "OPC", Command = 17 },
                new CommandAlias { Alias = "UID", Command = 19 },
                new CommandAlias { Alias = "Ignore", Command = 20 },
                new CommandAlias { Alias = "IgnoreUser", Command = 20 },
                new CommandAlias { Alias = "Block", Command = 20 },
                new CommandAlias { Alias = "Unblock", Command = 21 },
                new CommandAlias { Alias = "PermaVoiceMute", Command = 23 },
                new CommandAlias { Alias = "PVoiceMute", Command = 23 },
                new CommandAlias { Alias = "RVoiceMute", Command = 23 },
                new CommandAlias { Alias = "TimeVoiceMute", Command = 23 },
                new CommandAlias { Alias = "TVoiceMute", Command = 23 },
                new CommandAlias { Alias = "PermaMuteVoice", Command = 23 },
                new CommandAlias { Alias = "PMuteVoice", Command = 23 },
                new CommandAlias { Alias = "RMuteVoice", Command = 23 },
                new CommandAlias { Alias = "TimeMuteVoice", Command = 23 },
                new CommandAlias { Alias = "TMuteVoice", Command = 23 },
                new CommandAlias { Alias = "VoicePermaMute", Command = 23 },
                new CommandAlias { Alias = "VoicePMute", Command = 23 },
                new CommandAlias { Alias = "VoiceRMute", Command = 23 },
                new CommandAlias { Alias = "VoiceTimeMute", Command = 23 },
                new CommandAlias { Alias = "VoiceTMute", Command = 23 },
                new CommandAlias { Alias = "MuteVoice", Command = 23 },
                new CommandAlias { Alias = "MoneyGive", Command = 24 },
                new CommandAlias { Alias = "SendMoney", Command = 24 },
                new CommandAlias { Alias = "MoneySend", Command = 24 },
                new CommandAlias { Alias = "LobbyInvite", Command = 25 },
                new CommandAlias { Alias = "InviteLobby", Command = 25 },
                new CommandAlias { Alias = "InvitePlayerLobby", Command = 25 },
                new CommandAlias { Alias = "HouseCreate", Command = 27 },
                new CommandAlias { Alias = "NewHouse", Command = 27 },
                new CommandAlias { Alias = "HouseNew", Command = 27 }
            );

            return modelBuilder;
        }
    }
}
