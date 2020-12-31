using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Server.Data.Models;
using TDS.Server.Data.Utility;
using TDS.Server.Handler.Appearance;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.GangSystem;
using TDS.Server.Handler.Maps;
using TDS.Server.Handler.PlayerHandlers;
using TDS.Shared.Core;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Events
{
    public class RemoteBrowserEventsHandler
    {
        private readonly Dictionary<string, FromBrowserAsyncMethodDelegate> _asyncMethods;

        private readonly ILoggingHandler _loggingHandler;

        private readonly Dictionary<string, FromBrowserMaybeAsyncMethodDelegate> _maybeAsyncMethods;

        private readonly Dictionary<string, FromBrowserMethodDelegate> _methods;

        private readonly EventsHandler _eventsHandler;

        public RemoteBrowserEventsHandler(IUserpanelHandler userpanelHandler, LobbiesHandler lobbiesHandler, InvitationsHandler invitationsHandler,
            MapsLoadingHandler mapsLoadingHandler, ILoggingHandler loggingHandler, MapCreatorHandler mapCreatorHandler, MapFavouritesHandler mapFavouritesHandler,
            GangWindowHandler gangWindowHandler, EventsHandler eventsHandler, PlayerSettingsSyncHandler playerSettingsSyncHandler)
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
                [ToServerEvent.SaveUserpanelNormalSettings] = userpanelHandler.SettingsNormalHandler.SaveSettings,
                [ToServerEvent.SendApplication] = userpanelHandler.ApplicationUserHandler.CreateApplication,
                [ToServerEvent.SetSupportRequestClosed] = userpanelHandler.SupportRequestHandler.SetSupportRequestClosed,
                [ToServerEvent.SendSupportRequest] = userpanelHandler.SupportRequestHandler.SendRequest,
                [ToServerEvent.SendSupportRequestMessage] = userpanelHandler.SupportRequestHandler.SendMessage,
                [ToServerEvent.ToggleMapFavouriteState] = mapFavouritesHandler.ToggleMapFavouriteState,
                [ToServerEvent.SavePlayerCommandsSettings] = userpanelHandler.SettingsCommandsHandler.Save,
                [ToServerEvent.GangCommand] = gangWindowHandler.ExecuteCommand,
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
                [ToServerEvent.SetDamageTestWeaponDamage] = SetDamageTestWeaponDamage,
                [ToServerEvent.LoadUserpanelNormalSettingsData] = userpanelHandler.SettingsNormalHandler.LoadSettings,
                [ToServerEvent.ReloadPlayerSettings] = playerSettingsSyncHandler.RequestSyncPlayerSettingsFromUserpanel,
            };

            NAPI.ClientEvent.Register<ITDSPlayer, object[]>(ToServerEvent.FromBrowserEvent, this, OnFromBrowserEvent);
            NAPI.ClientEvent.Register<ITDSPlayer, object[]>(ToServerEvent.FromBrowserEventCallback, this, OnFromBrowserEventCallback);
        }

        public delegate Task<object?> FromBrowserAsyncMethodDelegate(ITDSPlayer player, ArraySegment<object> args);

        public delegate ValueTask<object?> FromBrowserMaybeAsyncMethodDelegate(ITDSPlayer player, ArraySegment<object> args);

        public delegate object? FromBrowserMethodDelegate(ITDSPlayer player, ref ArraySegment<object> args);

        private async void OnFromBrowserEvent(ITDSPlayer player, params object[] args)
        {
            var eventName = (string)args[0];
            var ret = await OnFromBrowserEventMethod(player, args);
            if (ret is { })
                NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.FromBrowserEventReturn, eventName, ret));
        }

        private async void OnFromBrowserEventCallback(ITDSPlayer player, params object[] args)
        {
            var eventName = (string)args[0];
            var ret = await OnFromBrowserEventMethod(player, args);
            NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.FromBrowserEventReturn, eventName, ret ?? ""));
        }

        private async Task<object?> OnFromBrowserEventMethod(ITDSPlayer player, params object[] args)
        {
            try
            {
                await Task.Yield();
                object? ret = null;

                var eventName = (string)args[0];
                var argsWithoutEventName = new ArraySegment<object>(args, 1, args.Length - 1);

                if (_asyncMethods.ContainsKey(eventName))
                {
                    ret = await _asyncMethods[eventName](player, argsWithoutEventName).ConfigureAwait(false);
                }
                else if (_maybeAsyncMethods.ContainsKey(eventName))
                {
                    ret = await _maybeAsyncMethods[eventName](player, argsWithoutEventName).ConfigureAwait(false);
                }
                else if (_methods.ContainsKey(eventName))
                {
                    ret = _methods[eventName](player, ref argsWithoutEventName);
                }

                return ret;
            }
            catch (Exception ex)
            {
                var baseEx = ex.GetBaseException();
                _loggingHandler.LogError(baseEx.Message + "\n"
                    + string.Join('\n', args.Select(a => Convert.ToString(a)?.Substring(0, Math.Min(Convert.ToString(a)?.Length ?? 0, 20)) ?? "-")),
                    ex.StackTrace ?? Environment.StackTrace, ex.GetType().Name + "|" + baseEx.GetType().Name, player);
                return null;
            }
        }

        public void AddSyncEvent(string eventName, FromBrowserMethodDelegate method)
        {
            _methods[eventName] = method;
        }

        public void AddAsyncEvent(string eventName, FromBrowserAsyncMethodDelegate method)
        {
            _asyncMethods[eventName] = method;
        }

        public void AddMaybeAsyncEvent(string eventName, FromBrowserMaybeAsyncMethodDelegate method)
        {
            _maybeAsyncMethods[eventName] = method;
        }

        public void RemoveSyncEvent(string eventName)
        {
            _methods.Remove(eventName);
        }

        public void RemoveAsyncEvent(string eventName)
        {
            _asyncMethods.Remove(eventName);
        }

        public void RemoveMaybeAsyncEvent(string eventName)
        {
            _maybeAsyncMethods.Remove(eventName);
        }

        private object? BuyMap(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (player.Lobby is null)
                return null;
            if (player.Lobby is not IArena arena)
                return null;
            int? mapId;
            if ((mapId = Utils.GetInt(args[0])) == null)
                return null;

            arena.MapVoting.BuyMap(player, mapId.Value);
            return null;
        }

        private object? GiveVehicle(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (player.Lobby is null || player.Lobby is not IFreeroamLobby lobby)
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
            if (player.Lobby is not IMapCreatorLobby lobby)
                return null;
            var infoType = (MapCreatorInfoType)Convert.ToInt32(args[0]);
            var data = args[1];

            lobby.Sync.SyncMapInfoChange(infoType, data);
            return null;
        }

        private object? MapVote(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (player.Lobby is not IArena arena)
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
            if (player.Lobby is not IDamageTestLobby damageTestLobby)
                return null;
            if (!player.IsLobbyOwner)
                return null;

            var weaponDamageData = Serializer.FromBrowser<DamageTestWeapon>((string)args[0]);
            damageTestLobby.Deathmatch.SetWeaponDamage(weaponDamageData);

            return null;
        }
    }
}