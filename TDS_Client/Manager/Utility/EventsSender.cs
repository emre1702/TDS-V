using RAGE;
using System.Collections.Generic;
using TDS_Client.Dto;
using TDS_Common.Default;

namespace TDS_Client.Manager.Utility
{
    internal static class EventsSender
    {
        private static readonly Dictionary<string, CooldownEventDto> cooldownEventsDict = new Dictionary<string, CooldownEventDto>
        {
            [DToServerEvent.AddRatingToMap] = new CooldownEventDto(DToServerEvent.AddRatingToMap, 1000),
            [DToServerEvent.ChatLoaded] = new CooldownEventDto(DToServerEvent.ChatLoaded, 100000),
            [DToServerEvent.CommandUsed] = new CooldownEventDto(DToServerEvent.CommandUsed, 500),
            [DToServerEvent.CreateCustomLobby] = new CooldownEventDto(DToServerEvent.CreateCustomLobby, 2000),
            [DToServerEvent.GetVehicle] = new CooldownEventDto(DToServerEvent.GetVehicle, 2000),
            [DToServerEvent.JoinArena] = new CooldownEventDto(DToServerEvent.JoinArena, 1000),
            [DToServerEvent.JoinLobby] = new CooldownEventDto(DToServerEvent.JoinLobby, 1000),
            [DToServerEvent.JoinLobbyWithPassword] = new CooldownEventDto(DToServerEvent.JoinLobbyWithPassword, 1000),
            [DToServerEvent.JoinMapCreator] = new CooldownEventDto(DToServerEvent.JoinMapCreator, 1000),
            [DToServerEvent.LanguageChange] = new CooldownEventDto(DToServerEvent.LanguageChange, 500),
            [DToServerEvent.LobbyChatMessage] = new CooldownEventDto(DToServerEvent.LobbyChatMessage, 250),
            [DToServerEvent.LoadMySavedMapNames] = new CooldownEventDto(DToServerEvent.LoadMySavedMapNames, 10000),
            [DToServerEvent.MapsListRequest] = new CooldownEventDto(DToServerEvent.MapsListRequest, 1000),
            [DToServerEvent.MapVote] = new CooldownEventDto(DToServerEvent.MapVote, 500),
            [DToServerEvent.RequestPlayersForScoreboard] = new CooldownEventDto(DToServerEvent.RequestPlayersForScoreboard, 5000),
            [DToServerEvent.SaveMapCreatorData] = new CooldownEventDto(DToServerEvent.SaveMapCreatorData, 10000),
            [DToServerEvent.SendMapCreatorData] = new CooldownEventDto(DToServerEvent.SendMapCreatorData, 10000),
            [DToServerEvent.SendMapRating] = new CooldownEventDto(DToServerEvent.SendMapRating, 2000),
            [DToServerEvent.SendTeamOrder] = new CooldownEventDto(DToServerEvent.SendTeamOrder, 2000),
            [DToServerEvent.ToggleMapFavouriteState] = new CooldownEventDto(DToServerEvent.ToggleMapFavouriteState, 500),
            [DToServerEvent.TryLogin] = new CooldownEventDto(DToServerEvent.TryLogin, 1000),
            [DToServerEvent.TryRegister] = new CooldownEventDto(DToServerEvent.TryRegister, 1000),
        };

        public static bool Send(string eventName, params object[] args)
        {
            if (!cooldownEventsDict.TryGetValue(eventName, out CooldownEventDto entry))
            {
                Events.CallRemote(eventName, args);
                return true;
            }

            ulong currentTicks = TimerManager.ElapsedTicks;
            if (entry.LastExecMs != 0 && currentTicks - entry.LastExecMs < entry.CooldownMs)
            {
                return false;
            }

            entry.LastExecMs = currentTicks;
            Events.CallRemote(eventName, args);
            return true;
        }
    }
}