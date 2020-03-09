﻿using AutoMapper;
using GTANetworkAPI;
using System.Linq;
using TDS_Common.Manager.Utility;

namespace TDS_Server.Core.Manager.Mapping.Converter
{
    class StringNameToClientConverter : ITypeConverter<string, Player?>
    {
        public Player? Convert(string name, Player? destination, ResolutionContext _)
        {
            if (name[0] == '@')
                name = name.Substring(1);

            Player? player = NAPI.Player.GetPlayerFromName(name);
            if (player != null)
                return player;

            player = NAPI.Player.GetPlayerFromName(Constants.ServerTeamSuffix + name);
            if (player != null)
                return player;

            name = name.ToLower();
            return NAPI.Pools.GetAllPlayers().FirstOrDefault(c => c.Name.ToLower().StartsWith(name) || c.Name.ToLower().StartsWith(Constants.ServerTeamSuffix + name));
        }
    }
}