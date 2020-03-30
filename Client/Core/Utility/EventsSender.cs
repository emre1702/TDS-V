using RAGE;
using System.Collections.Generic;
using TDS_Client.Dto;
using TDS_Shared.Default;

namespace TDS_Client.Manager.Utility
{
    internal static class EventsSender
    {
        private static readonly Dictionary<string, CooldownEventDto> _cooldownEventsDict = new Dictionary<string, CooldownEventDto>
        {
            [ToServerEvent.AddRatingToMap] = new CooldownEventDto(1000),
            [ToServerEvent.BuyMap] = new CooldownEventDto(2000),
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
            [ToServerEvent.SaveMapCreatorData] = new CooldownEventDto(10000),
            [ToServerEvent.SaveSettings] = new CooldownEventDto(3000),
            [ToServerEvent.SendApplication] = new CooldownEventDto(3000),
            [ToServerEvent.SendApplicationInvite] = new CooldownEventDto(5000),
            [ToServerEvent.SendMapCreatorData] = new CooldownEventDto(10000),
            [ToServerEvent.SendMapRating] = new CooldownEventDto(2000),
            [ToServerEvent.SendSupportRequest] = new CooldownEventDto(2000),
            [ToServerEvent.SendSupportRequestMessage] = new CooldownEventDto(300),
            [ToServerEvent.SendTeamOrder] = new CooldownEventDto(2000),
            [ToServerEvent.ToggleMapFavouriteState] = new CooldownEventDto(500),
            [ToServerEvent.TryLogin] = new CooldownEventDto(1000),
            [ToServerEvent.TryRegister] = new CooldownEventDto(1000),
        };

        public static bool Send(string eventName, params object[] args)
        {
            if (!_cooldownEventsDict.TryGetValue(eventName, out CooldownEventDto entry))
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

        /// <summary>
        /// Still saves the last execute time for .Send cooldown.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool SendIgnoreCooldown(string eventName, params object[] args)
        {
            if (!_cooldownEventsDict.TryGetValue(eventName, out CooldownEventDto entry))
            {
                Events.CallRemote(eventName, args);
                return true;
            }

            ulong currentTicks = TimerManager.ElapsedTicks;
            entry.LastExecMs = currentTicks;
            Events.CallRemote(eventName, args);
            return true;
        }

        public static bool SendFromBrowser(params object[] args)
        {
            string eventName = (string)args[0];
            if (!_cooldownEventsDict.TryGetValue(eventName, out CooldownEventDto entry))
            {
                RAGE.Events.CallRemote(ToServerEvent.FromBrowserEvent, args);
                return true;
            }

            ulong currentTicks = TimerManager.ElapsedTicks;
            if (entry.LastExecMs != 0 && currentTicks - entry.LastExecMs < entry.CooldownMs)
            {
                return false;
            }

            entry.LastExecMs = currentTicks;
            RAGE.Events.CallRemote(ToServerEvent.FromBrowserEvent, args);
            return true;
        }

        private static void CallWithParamsArgs(string eventName, params object[] args)
        {
            switch (args.Length)
            {
                case 0:
                    RAGE.Chat.Output("0");
                    Events.CallRemote(eventName);
                    break;
                case 1:
                    RAGE.Chat.Output("1");
                    Events.CallRemote(eventName, args[0]);
                    break;
                case 2:
                    RAGE.Chat.Output("2");
                    Events.CallRemote(eventName, args[0], args[1]);
                    break;
                case 3:
                    Events.CallRemote(eventName, args[0], args[1], args[2]);
                    break;
                case 4:
                    Events.CallRemote(eventName, args[0], args[1], args[2], args[3]);
                    break;
                case 5:
                    Events.CallRemote(eventName, args[0], args[1], args[2], args[3], args[4]);
                    break;
                case 6:
                    Events.CallRemote(eventName, args[0], args[1], args[2], args[3], args[4], args[5]);
                    break;
                case 7:
                    Events.CallRemote(eventName, args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
                    break;
                case 8:
                    Events.CallRemote(eventName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]);
                    break;
                case 9:
                    Events.CallRemote(eventName, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]);
                    break;
            }
                
        }
    }
}
