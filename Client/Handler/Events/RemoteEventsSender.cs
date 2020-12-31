using System.Collections.Generic;
using TDS.Client.Data.Models;
using TDS.Shared.Default;

namespace TDS.Client.Handler.Events
{
    public class RemoteEventsSender : ServiceBase
    {
        private readonly Dictionary<string, CooldownEventDto> _cooldownEventsDict = new Dictionary<string, CooldownEventDto>
        {
            [ToServerEvent.AddRatingToMap] = new CooldownEventDto(1000),
            [ToServerEvent.BuyMap] = new CooldownEventDto(2000),
            [ToServerEvent.CancelCharCreateData] = new CooldownEventDto(1000),
            [ToServerEvent.ChooseTeam] = new CooldownEventDto(1000),
            [ToServerEvent.CommandUsed] = new CooldownEventDto(500),
            [ToServerEvent.CreateCustomLobby] = new CooldownEventDto(2000),
            [ToServerEvent.GetSupportRequestData] = new CooldownEventDto(1000),
            [ToServerEvent.GetVehicle] = new CooldownEventDto(2000),
            [ToServerEvent.JoinLobby] = new CooldownEventDto(3000),
            [ToServerEvent.JoinLobbyWithPassword] = new CooldownEventDto(1000),
            [ToServerEvent.LanguageChange] = new CooldownEventDto(500),
            [ToServerEvent.LeaveLobby] = new CooldownEventDto(500),
            [ToServerEvent.LoadApplicationDataForAdmin] = new CooldownEventDto(3000),
            [ToServerEvent.LobbyChatMessage] = new CooldownEventDto(250),
            [ToServerEvent.LoadMapForMapCreator] = new CooldownEventDto(10000),
            [ToServerEvent.LoadMapNamesToLoadForMapCreator] = new CooldownEventDto(10000),
            [ToServerEvent.LoadUserpanelData] = new CooldownEventDto(1000),
            [ToServerEvent.MapsListRequest] = new CooldownEventDto(1000),
            [ToServerEvent.MapVote] = new CooldownEventDto(500),
            [ToServerEvent.RequestPlayersForScoreboard] = new CooldownEventDto(5000),
            [ToServerEvent.ResetPassword] = new CooldownEventDto(4000),
            [ToServerEvent.SaveBodyData] = new CooldownEventDto(10000),
            [ToServerEvent.SaveClothesData] = new CooldownEventDto(10000),
            [ToServerEvent.SaveMapCreatorData] = new CooldownEventDto(10000),
            [ToServerEvent.SaveUserpanelNormalSettings] = new CooldownEventDto(3000),
            [ToServerEvent.SendApplication] = new CooldownEventDto(3000),
            [ToServerEvent.SendApplicationInvite] = new CooldownEventDto(5000),
            [ToServerEvent.SendMapCreatorData] = new CooldownEventDto(10000),
            [ToServerEvent.SendMapRating] = new CooldownEventDto(2000),
            [ToServerEvent.SendSupportRequest] = new CooldownEventDto(2000),
            [ToServerEvent.SendSupportRequestMessage] = new CooldownEventDto(300),
            [ToServerEvent.SendTeamOrder] = new CooldownEventDto(2000),
            [ToServerEvent.ToggleMapFavouriteState] = new CooldownEventDto(500),
            [ToServerEvent.TryLogin] = new CooldownEventDto(4000),
            [ToServerEvent.TryRegister] = new CooldownEventDto(4000),
        };

        private readonly TimerHandler _timerHandler;

        public RemoteEventsSender(LoggingHandler loggingHandler, TimerHandler timerHandler)
            : base(loggingHandler) => _timerHandler = timerHandler;

        public bool Send(string eventName, params object[] args)
        {
            if (!_cooldownEventsDict.TryGetValue(eventName, out CooldownEventDto entry))
            {
                RAGE.Events.CallRemote(eventName, args);
                return true;
            }

            int currentTicks = _timerHandler.ElapsedMs;
            if (entry.LastExecMs != 0 && currentTicks - entry.LastExecMs < entry.CooldownMs)
            {
                return false;
            }

            entry.LastExecMs = currentTicks;
            RAGE.Events.CallRemote(eventName, args);
            return true;
        }

        public bool SendFromBrowser(params object[] args)
        {
            return SendFromBrowserMethod(ToServerEvent.FromBrowserEvent, args);
        }

        public bool SendFromBrowserCallback(params object[] args)
        {
            return SendFromBrowserMethod(ToServerEvent.FromBrowserEventCallback, args);
        }

        private bool SendFromBrowserMethod(string toServerEventName, params object[] args)
        {
            string eventName = (string)args[0];
            if (!_cooldownEventsDict.TryGetValue(eventName, out CooldownEventDto entry))
            {
                RAGE.Events.CallRemote(toServerEventName, args);
                return true;
            }

            int currentTicks = _timerHandler.ElapsedMs;
            if (entry.LastExecMs != 0 && currentTicks - entry.LastExecMs < entry.CooldownMs)
            {
                return false;
            }

            entry.LastExecMs = currentTicks;
            RAGE.Events.CallRemote(toServerEventName, args);
            return true;
        }

        /// <summary>
        /// Still saves the last execute time for .Send cooldown.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool SendIgnoreCooldown(string eventName, params object[] args)
        {
            if (!_cooldownEventsDict.TryGetValue(eventName, out CooldownEventDto entry))
            {
                RAGE.Events.CallRemote(eventName, args);
                return true;
            }

            int currentTicks = _timerHandler.ElapsedMs;
            entry.LastExecMs = currentTicks;
            RAGE.Events.CallRemote(eventName, args);
            return true;
        }
    }
}