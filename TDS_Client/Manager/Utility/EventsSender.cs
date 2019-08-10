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
            [DToServerEvent.AddRatingToMap] = new CooldownEventDto(1000),
            [DToServerEvent.ChatLoaded] = new CooldownEventDto(100000),
            [DToServerEvent.ChooseTeam] = new CooldownEventDto(1000),
            [DToServerEvent.CommandUsed] = new CooldownEventDto(500),
            [DToServerEvent.CreateCustomLobby] = new CooldownEventDto(2000),
            [DToServerEvent.GetVehicle] = new CooldownEventDto(2000),
            [DToServerEvent.JoinArena] = new CooldownEventDto(1000),
            [DToServerEvent.JoinLobby] = new CooldownEventDto(1000),
            [DToServerEvent.JoinLobbyWithPassword] = new CooldownEventDto(1000),
            [DToServerEvent.JoinMapCreator] = new CooldownEventDto(1000),
            [DToServerEvent.LanguageChange] = new CooldownEventDto(500),
            [DToServerEvent.LobbyChatMessage] = new CooldownEventDto(250),
            [DToServerEvent.LoadAllCommands] = new CooldownEventDto(10000),
            [DToServerEvent.LoadAllRules] = new CooldownEventDto(10000),
            [DToServerEvent.LoadMapForMapCreator] = new CooldownEventDto(10000),
            [DToServerEvent.LoadMapNamesToLoadForMapCreator] = new CooldownEventDto(10000),
            [DToServerEvent.MapsListRequest] = new CooldownEventDto(1000),
            [DToServerEvent.MapVote] = new CooldownEventDto(500),
            [DToServerEvent.RequestPlayersForScoreboard] = new CooldownEventDto(5000),
            [DToServerEvent.SaveMapCreatorData] = new CooldownEventDto(10000),
            [DToServerEvent.SendMapCreatorData] = new CooldownEventDto(10000),
            [DToServerEvent.SendMapRating] = new CooldownEventDto(2000),
            [DToServerEvent.SendTeamOrder] = new CooldownEventDto(2000),
            [DToServerEvent.ToggleMapFavouriteState] = new CooldownEventDto(500),
            [DToServerEvent.TryLogin] = new CooldownEventDto(1000),
            [DToServerEvent.TryRegister] = new CooldownEventDto(1000),
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