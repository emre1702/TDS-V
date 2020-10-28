using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Data.Models;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.PlayerHandlers;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Events
{
    public class RemoteBrowserEventsHandler
    {
        private readonly Dictionary<string, FromBrowserAsyncMethodDelegate> _asyncMethods;

        private readonly ILoggingHandler _loggingHandler;

        private readonly Dictionary<string, FromBrowserMaybeAsyncMethodDelegate> _maybeAsyncMethods;

        private readonly Dictionary<string, FromBrowserMethodDelegate> _methods;

        private readonly EventsHandler _eventsHandler;

        public RemoteBrowserEventsHandler(IUserpanelHandler userpanelHandler, LobbiesHandler lobbiesHandler, InvitationsHandler invitationsHandler, MapsLoadingHandler mapsLoadingHandler,
            ILoggingHandler loggingHandler, MapCreatorHandler mapCreatorHandler,
            MapFavouritesHandler mapFavouritesHandler, PlayerCharHandler playerCharHandler,
            GangWindowHandler gangWindowHandler, EventsHandler eventsHandler)
        {
            _loggingHandler = loggingHandler;
            _eventsHandler = eventsHandler;

            _asyncMethods = new Dictionary<string, FromBrowserAsyncMethodDelegate>
            {
                [ToServerEvent.SendApplicationInvite] = userpanelHandler.ApplicationsAdminHandler.SendInvitation,
                [ToServerEvent.AnswerToOfflineMessage] = userpanelHandler.OfflineMessagesHandler.Answer,
                [ToServerEvent.SendOfflineMessage] = userpanelHandler.OfflineMessagesHandler.Send,
                [ToServerEvent.DeleteOfflineMessage] = userpanelHandler.OfflineMessagesHandler.Delete,
                [ToServerEvent.SaveSpecialSettingsChange] = userpanelHandler.SettingsSpecialHandler.SetData,
                [ToServerEvent.AcceptTDSTeamInvitation] = userpanelHandler.ApplicationUserHandler.AcceptInvitation,
                [ToServerEvent.RejectTDSTeamInvitation] = userpanelHandler.ApplicationUserHandler.RejectInvitation,
                [ToServerEvent.CreateCustomLobby] = lobbiesHandler.CreateCustomLobby,
                [ToServerEvent.GetSupportRequestData] = userpanelHandler.SupportRequestHandler.GetSupportRequestData,
                [ToServerEvent.JoinLobby] = lobbiesHandler.OnJoinLobby,
                [ToServerEvent.JoinLobbyWithPassword] = lobbiesHandler.OnJoinLobbyWithPassword,
                [ToServerEvent.LoadApplicationDataForAdmin] = userpanelHandler.ApplicationsAdminHandler.SendApplicationData,
                [ToServerEvent.SaveMapCreatorData] = mapCreatorHandler.Save,
                [ToServerEvent.SendMapCreatorData] = mapCreatorHandler.Create,
                [ToServerEvent.SaveSettings] = userpanelHandler.SettingsNormalHandler.SaveSettings,
                [ToServerEvent.SendApplication] = userpanelHandler.ApplicationUserHandler.CreateApplication,
                [ToServerEvent.SetSupportRequestClosed] = userpanelHandler.SupportRequestHandler.SetSupportRequestClosed,
                [ToServerEvent.SendSupportRequest] = userpanelHandler.SupportRequestHandler.SendRequest,
                [ToServerEvent.SendSupportRequestMessage] = userpanelHandler.SupportRequestHandler.SendMessage,
                [ToServerEvent.ToggleMapFavouriteState] = mapFavouritesHandler.ToggleMapFavouriteState,
                [ToServerEvent.SaveCharCreateData] = playerCharHandler.Save,
                [ToServerEvent.CancelCharCreateData] = playerCharHandler.Cancel,
                [ToServerEvent.SavePlayerCommandsSettings] = userpanelHandler.SettingsCommandsHandler.Save,
                [ToServerEvent.GangCommand] = gangWindowHandler.ExecuteCommand
            };

            _maybeAsyncMethods = new Dictionary<string, FromBrowserMaybeAsyncMethodDelegate>
            {
                [ToServerEvent.LoadDatasForCustomLobby] = lobbiesHandler.LoadDatas
            };

            _methods = new Dictionary<string, FromBrowserMethodDelegate>
            {
                [ToServerEvent.BuyMap] = BuyMap,
                [ToServerEvent.MapCreatorSyncData] = MapCreatorSyncData,
                [ToServerEvent.AcceptInvitation] = invitationsHandler.AcceptInvitation,
                [ToServerEvent.RejectInvitation] = invitationsHandler.RejectInvitation,
                [ToServerEvent.LoadAllMapsForCustomLobby] = mapsLoadingHandler.GetAllMapsForCustomLobby,
                [ToServerEvent.MapVote] = MapVote,
                [ToServerEvent.GetVehicle] = GiveVehicle,
                [ToServerEvent.JoinedCustomLobbiesMenu] = JoinedCustomLobbiesMenu,
                [ToServerEvent.LeftCustomLobbiesMenu] = LeftCustomLobbiesMenu,
                [ToServerEvent.LeftSupportRequest] = userpanelHandler.SupportRequestHandler.LeftSupportRequest,
                [ToServerEvent.LeftSupportRequestsList] = userpanelHandler.SupportRequestHandler.LeftSupportRequestsList,
                [ToServerEvent.LoadMapNamesToLoadForMapCreator] = mapCreatorHandler.SendPlayerMapNamesForMapCreator,
                [ToServerEvent.LoadMapForMapCreator] = mapCreatorHandler.SendPlayerMapForMapCreator,
                [ToServerEvent.MapCreatorSyncCurrentMapToServer] = mapCreatorHandler.SyncCurrentMapToClient,
                [ToServerEvent.LoadPlayerWeaponStats] = userpanelHandler.PlayerWeaponStatsHandler.GetPlayerWeaponStats,
                [ToServerEvent.LoadGangWindowData] = gangWindowHandler.OnLoadGangWindowData,
                [ToServerEvent.SetDamageTestWeaponDamage] = SetDamageTestWeaponDamage
            };

            NAPI.ClientEvent.Register<ITDSPlayer, object[]>(ToServerEvent.FromBrowserEvent, this, OnFromBrowserEvent);
        }

        public delegate Task<object?> FromBrowserAsyncMethodDelegate(ITDSPlayer player, ArraySegment<object> args);

        public delegate ValueTask<object?> FromBrowserMaybeAsyncMethodDelegate(ITDSPlayer player, ArraySegment<object> args);

        public delegate object? FromBrowserMethodDelegate(ITDSPlayer player, ref ArraySegment<object> args);

        public async void OnFromBrowserEvent(ITDSPlayer player, params object[] args)
        {
            if (!player.LoggedIn)
                return;
            try
            {
                object? ret = null;

                var eventName = (string)args[0];
                var argsWithoutEventName = new ArraySegment<object>(args, 1, args.Length - 1);

                if (_asyncMethods.ContainsKey(eventName))
                {
                    ret = await _asyncMethods[eventName](player, argsWithoutEventName);
                }
                else if (_maybeAsyncMethods.ContainsKey(eventName))
                {
                    ret = await _maybeAsyncMethods[eventName](player, argsWithoutEventName);
                }

                NAPI.Task.RunSafe(() =>
                {
                    if (_methods.ContainsKey(eventName))
                    {
                        ret = _methods[eventName](player, ref argsWithoutEventName);
                    }

                    if (ret != null)
                    {
                        player.TriggerEvent(ToClientEvent.FromBrowserEventReturn, eventName, ret);
                    }
                });
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                _loggingHandler.LogError(baseEx.Message + "\n"
                    + String.Join('\n', args.Select(a => Convert.ToString(a)?.Substring(0, Math.Min(Convert.ToString(a)?.Length ?? 0, 20)) ?? "-")),
                    ex.StackTrace ?? Environment.StackTrace, ex.GetType().Name + "|" + baseEx.GetType().Name, player);
            }
        }

        private object? BuyMap(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (player.Lobby is null)
                return null;
            if (!(player.Lobby is IArena arena))
                return null;
            int? mapId;
            if ((mapId = Utils.GetInt(args[0])) == null)
                return null;

            arena.MapVoting.BuyMap(player, mapId.Value);
            return null;
        }

        private object? GiveVehicle(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (player.Lobby is null || !(player.Lobby is IFreeroamLobby lobby))
                return null;

            if (args.Count == 0)
                return null;

            if (!Enum.TryParse(args[0].ToString(), out FreeroamVehicleType vehType))
                return null;

            lobby.Freeroam.GiveVehicle(player, vehType);
            return null;
        }

        private object? JoinedCustomLobbiesMenu(ITDSPlayer player, ref ArraySegment<object> args)
        {
            _eventsHandler.OnCustomLobbyMenuJoin(player);
            return null;
        }

        private object? LeftCustomLobbiesMenu(ITDSPlayer player, ref ArraySegment<object> args)
        {
            _eventsHandler.OnCustomLobbyMenuLeave(player);
            return null;
        }

        private object? MapCreatorSyncData(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (player.Lobby is null)
                return null;
            if (!(player.Lobby is IMapCreatorLobby lobby))
                return null;
            var infoType = (MapCreatorInfoType)Convert.ToInt32(args[0]);
            var data = args[1];

            lobby.Sync.SyncMapInfoChange(infoType, data);
            return null;
        }

        private object? MapVote(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (!(player.Lobby is IArena arena))
                return null;

            if (args.Count == 0)
                return null;

            int? mapId;
            if ((mapId = Utils.GetInt(args[0])) == null)
                return null;

            arena.MapVoting.VoteForMap(player, mapId.Value);
            return null;
        }

        private object? SetDamageTestWeaponDamage(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (!(player.Lobby is IDamageTestLobby damageTestLobby))
                return null;
            if (!player.IsLobbyOwner)
                return null;

            var weaponDamageData = Serializer.FromBrowser<DamageTestWeapon>((string)args[0]);
            damageTestLobby.Deathmatch.SetWeaponDamage(weaponDamageData);

            return null;
        }
    }
}
