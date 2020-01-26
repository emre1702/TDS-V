using RAGE;
using System.Collections.Generic;
using TDS_Client.Dto;
using TDS_Common.Default;

namespace TDS_Client.Manager.Utility
{
    internal static class EventsSender
    {
        private static readonly Dictionary<string, CooldownEventDto> _cooldownEventsDict = new Dictionary<string, CooldownEventDto>
        {
            [DToServerEvent.AddRatingToMap] = new CooldownEventDto(1000),
            [DToServerEvent.BuyMap] = new CooldownEventDto(2000),
            [DToServerEvent.ChatLoaded] = new CooldownEventDto(100000),
            [DToServerEvent.ChooseTeam] = new CooldownEventDto(1000),
            [DToServerEvent.CommandUsed] = new CooldownEventDto(500),
            [DToServerEvent.CreateCustomLobby] = new CooldownEventDto(2000),
            [DToServerEvent.GetSupportRequestData] = new CooldownEventDto(1000),
            [DToServerEvent.GetVehicle] = new CooldownEventDto(2000),
            [DToServerEvent.JoinArena] = new CooldownEventDto(1000),
            [DToServerEvent.JoinLobby] = new CooldownEventDto(1000),
            [DToServerEvent.JoinLobbyWithPassword] = new CooldownEventDto(1000),
            [DToServerEvent.JoinMapCreator] = new CooldownEventDto(1000),
            [DToServerEvent.LanguageChange] = new CooldownEventDto(500),
            [DToServerEvent.LeaveLobby] = new CooldownEventDto(500),
            [DToServerEvent.LoadApplicationDataForAdmin] = new CooldownEventDto(3000),
            [DToServerEvent.LobbyChatMessage] = new CooldownEventDto(250),
            [DToServerEvent.LoadMapForMapCreator] = new CooldownEventDto(10000),
            [DToServerEvent.LoadMapNamesToLoadForMapCreator] = new CooldownEventDto(10000),
            [DToServerEvent.LoadUserpanelData] = new CooldownEventDto(1000),
            [DToServerEvent.MapsListRequest] = new CooldownEventDto(1000),
            [DToServerEvent.MapVote] = new CooldownEventDto(500),
            [DToServerEvent.RequestPlayersForScoreboard] = new CooldownEventDto(5000),
            [DToServerEvent.SaveMapCreatorData] = new CooldownEventDto(10000),
            [DToServerEvent.SaveSettings] = new CooldownEventDto(3000),
            [DToServerEvent.SendApplication] = new CooldownEventDto(3000),
            [DToServerEvent.SendApplicationInvite] = new CooldownEventDto(5000),
            [DToServerEvent.SendMapCreatorData] = new CooldownEventDto(10000),
            [DToServerEvent.SendMapRating] = new CooldownEventDto(2000),
            [DToServerEvent.SendSupportRequest] = new CooldownEventDto(2000),
            [DToServerEvent.SendSupportRequestMessage] = new CooldownEventDto(300),
            [DToServerEvent.SendTeamOrder] = new CooldownEventDto(2000),
            [DToServerEvent.ToggleMapFavouriteState] = new CooldownEventDto(500),
            [DToServerEvent.TryLogin] = new CooldownEventDto(1000),
            [DToServerEvent.TryRegister] = new CooldownEventDto(1000),
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
                RAGE.Events.CallRemote(DToServerEvent.FromBrowserEvent, args);
                return true;
            }

            ulong currentTicks = TimerManager.ElapsedTicks;
            if (entry.LastExecMs != 0 && currentTicks - entry.LastExecMs < entry.CooldownMs)
            {
                return false;
            }

            entry.LastExecMs = currentTicks;
            RAGE.Events.CallRemote(DToServerEvent.FromBrowserEvent, args);
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
