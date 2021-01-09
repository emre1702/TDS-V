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
        private readonly Dictionary<string, List<FromBrowserAsyncMethodDelegate>> _asyncMethods;

        private readonly ILoggingHandler _loggingHandler;

        private readonly Dictionary<string, List<FromBrowserMaybeAsyncMethodDelegate>> _maybeAsyncMethods;

        private readonly Dictionary<string, List<FromBrowserMethodDelegate>> _methods;

        private readonly EventsHandler _eventsHandler;

        public RemoteBrowserEventsHandler(IUserpanelHandler userpanelHandler, LobbiesHandler lobbiesHandler, InvitationsHandler invitationsHandler,
            MapsLoadingHandler mapsLoadingHandler, ILoggingHandler loggingHandler, MapCreatorHandler mapCreatorHandler, MapFavouritesHandler mapFavouritesHandler,
            GangWindowHandler gangWindowHandler, EventsHandler eventsHandler, PlayerSettingsSyncHandler playerSettingsSyncHandler)
        {
            _loggingHandler = loggingHandler;
            _eventsHandler = eventsHandler;

            _asyncMethods = new()
            {
                [ToServerEvent.SendApplicationInvite] = new() { userpanelHandler.ApplicationsAdminHandler.SendInvitation },
                [ToServerEvent.AnswerToOfflineMessage] = new() { userpanelHandler.OfflineMessagesHandler.Answer },
                [ToServerEvent.SendOfflineMessage] = new() { userpanelHandler.OfflineMessagesHandler.Send },
                [ToServerEvent.DeleteOfflineMessage] = new() { userpanelHandler.OfflineMessagesHandler.Delete },
                [ToServerEvent.SaveSpecialSettingsChange] = new() { userpanelHandler.SettingsSpecialHandler.SetData },
                [ToServerEvent.AcceptTDSTeamInvitation] = new() { userpanelHandler.ApplicationUserHandler.AcceptInvitation },
                [ToServerEvent.RejectTDSTeamInvitation] = new() { userpanelHandler.ApplicationUserHandler.RejectInvitation },
                [ToServerEvent.CreateCustomLobby] = new() { lobbiesHandler.CreateCustomLobby },
                [ToServerEvent.GetSupportRequestData] = new() { userpanelHandler.SupportRequestHandler.GetSupportRequestData },
                [ToServerEvent.JoinLobby] = new() { lobbiesHandler.OnJoinLobby },
                [ToServerEvent.JoinLobbyWithPassword] = new() { lobbiesHandler.OnJoinLobbyWithPassword },
                [ToServerEvent.LoadApplicationDataForAdmin] = new() { userpanelHandler.ApplicationsAdminHandler.SendApplicationData },
                [ToServerEvent.SaveMapCreatorData] = new() { mapCreatorHandler.Save },
                [ToServerEvent.SendMapCreatorData] = new() { mapCreatorHandler.Create },
                [ToServerEvent.SaveUserpanelNormalSettings] = new() { userpanelHandler.SettingsNormalHandler.SaveSettings },
                [ToServerEvent.SendApplication] = new() { userpanelHandler.ApplicationUserHandler.CreateApplication },
                [ToServerEvent.SetSupportRequestClosed] = new() { userpanelHandler.SupportRequestHandler.SetSupportRequestClosed },
                [ToServerEvent.SendSupportRequest] = new() { userpanelHandler.SupportRequestHandler.SendRequest },
                [ToServerEvent.SendSupportRequestMessage] = new() { userpanelHandler.SupportRequestHandler.SendMessage },
                [ToServerEvent.ToggleMapFavouriteState] = new() { mapFavouritesHandler.ToggleMapFavouriteState },
                [ToServerEvent.SavePlayerCommandsSettings] = new() { userpanelHandler.SettingsCommandsHandler.Save },
                [ToServerEvent.GangCommand] = new() { gangWindowHandler.ExecuteCommand },
            };

            _maybeAsyncMethods = new()
            {
                [ToServerEvent.LoadDatasForCustomLobby] = new() { lobbiesHandler.LoadDatas }
            };

            _methods = new()
            {
                [ToServerEvent.BuyMap] = new() { BuyMap },
                [ToServerEvent.MapCreatorSyncData] = new() { MapCreatorSyncData },
                [ToServerEvent.AcceptInvitation] = new() { invitationsHandler.AcceptInvitation },
                [ToServerEvent.RejectInvitation] = new() { invitationsHandler.RejectInvitation },
                [ToServerEvent.LoadAllMapsForCustomLobby] = new() { mapsLoadingHandler.GetAllMapsForCustomLobby },
                [ToServerEvent.MapVote] = new() { MapVote },
                [ToServerEvent.GetVehicle] = new() { GiveVehicle },
                [ToServerEvent.JoinedCustomLobbiesMenu] = new() { JoinedCustomLobbiesMenu },
                [ToServerEvent.LeftCustomLobbiesMenu] = new() { LeftCustomLobbiesMenu },
                [ToServerEvent.LeftSupportRequest] = new() { userpanelHandler.SupportRequestHandler.LeftSupportRequest },
                [ToServerEvent.LeftSupportRequestsList] = new() { userpanelHandler.SupportRequestHandler.LeftSupportRequestsList },
                [ToServerEvent.LoadMapNamesToLoadForMapCreator] = new() { mapCreatorHandler.SendPlayerMapNamesForMapCreator },
                [ToServerEvent.LoadMapForMapCreator] = new() { mapCreatorHandler.SendPlayerMapForMapCreator },
                [ToServerEvent.MapCreatorSyncCurrentMapToServer] = new() { mapCreatorHandler.SyncCurrentMapToClient },
                [ToServerEvent.LoadPlayerWeaponStats] = new() { userpanelHandler.PlayerWeaponStatsHandler.GetPlayerWeaponStats },
                [ToServerEvent.LoadGangWindowData] = new() { gangWindowHandler.OnLoadGangWindowData },
                [ToServerEvent.SetDamageTestWeaponDamage] = new() { SetDamageTestWeaponDamage },
                [ToServerEvent.LoadUserpanelNormalSettingsData] = new() { userpanelHandler.SettingsNormalHandler.LoadSettings },
                [ToServerEvent.ReloadPlayerSettings] = new() { playerSettingsSyncHandler.RequestSyncPlayerSettingsFromUserpanel },
            };

            NAPI.ClientEvent.Register<ITDSPlayer, object[]>(ToServerEvent.FromBrowserEvent, this, OnFromBrowserEvent);
            NAPI.ClientEvent.Register<ITDSPlayer, object[]>(ToServerEvent.FromBrowserEventCallback, this, OnFromBrowserEventCallback);
        }

        public delegate Task<object?> FromBrowserAsyncMethodDelegate(ITDSPlayer player, ArraySegment<object> args);

        public delegate ValueTask<object?> FromBrowserMaybeAsyncMethodDelegate(ITDSPlayer player, ArraySegment<object> args);

        public delegate object? FromBrowserMethodDelegate(ITDSPlayer player, ref ArraySegment<object> args);

        private async void OnFromBrowserEvent(ITDSPlayer player, params object[] args)
        {
            if (player is null)
                return;
            var eventName = (string)args[0];
            var ret = await OnFromBrowserEventMethod(player, args);
            if (ret is { })
                NAPI.Task.RunSafe(() => player.TriggerEvent(ToClientEvent.FromBrowserEventReturn, eventName, ret));
        }

        private async void OnFromBrowserEventCallback(ITDSPlayer player, params object[] args)
        {
            if (player is null)
                return;
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
                    foreach (var e in _asyncMethods[eventName])
                        ret ??= await e(player, argsWithoutEventName).ConfigureAwait(false);
                }
                if (_maybeAsyncMethods.ContainsKey(eventName))
                {
                    foreach (var e in _maybeAsyncMethods[eventName])
                        ret ??= await e(player, argsWithoutEventName).ConfigureAwait(false);
                }
                if (_methods.ContainsKey(eventName))
                {
                    foreach (var e in _methods[eventName])
                        ret ??= e(player, ref argsWithoutEventName);
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
            lock (_methods)
            {
                if (!_methods.TryGetValue(eventName, out var list))
                {
                    list = new();
                    _methods[eventName] = list;
                }

                list.Add(method);
            }
        }

        public void AddAsyncEvent(string eventName, FromBrowserAsyncMethodDelegate method)
        {
            lock (_asyncMethods)
            {
                if (!_asyncMethods.TryGetValue(eventName, out var list))
                {
                    list = new();
                    _asyncMethods[eventName] = list;
                }

                list.Add(method);
            }
        }

        public void AddMaybeAsyncEvent(string eventName, FromBrowserMaybeAsyncMethodDelegate method)
        {
            lock (_maybeAsyncMethods)
            {
                if (!_maybeAsyncMethods.TryGetValue(eventName, out var list))
                {
                    list = new();
                    _maybeAsyncMethods[eventName] = list;
                }

                list.Add(method);
            }
        }

        public void RemoveSyncEvent(string eventName, FromBrowserMethodDelegate method)
        {
            lock (_methods)
            {
                if (!_methods.TryGetValue(eventName, out var list))
                    return;
                list.Remove(method);
                if (list.Count == 0)
                    _methods.Remove(eventName);
            }
        }

        public void RemoveAsyncEvent(string eventName, FromBrowserAsyncMethodDelegate method)
        {
            lock (_asyncMethods)
            {
                if (!_asyncMethods.TryGetValue(eventName, out var list))
                    return;
                list.Remove(method);
                if (list.Count == 0)
                    _asyncMethods.Remove(eventName);
            }
        }

        public void RemoveMaybeAsyncEvent(string eventName, FromBrowserMaybeAsyncMethodDelegate method)
        {
            lock (_maybeAsyncMethods)
            {
                if (!_maybeAsyncMethods.TryGetValue(eventName, out var list))
                    return;
                list.Remove(method);
                if (list.Count == 0)
                    _maybeAsyncMethods.Remove(eventName);
            }
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