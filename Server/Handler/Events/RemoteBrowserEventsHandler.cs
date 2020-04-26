using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Sync;
using TDS_Server.Handler.Userpanel;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Events
{
    public class RemoteBrowserEventsHandler
    {

        public delegate Task<object?> FromBrowserAsyncMethodDelegate(ITDSPlayer player, object[] args);
        public delegate ValueTask<object?> FromBrowserMaybeAsyncMethodDelegate(ITDSPlayer player, object[] args);
        public delegate object? FromBrowserMethodDelegate(ITDSPlayer player, object[] args);

        private readonly Dictionary<string, FromBrowserAsyncMethodDelegate> _asyncMethods;
        private readonly Dictionary<string, FromBrowserMaybeAsyncMethodDelegate> _maybeAsyncMethods;
        private readonly Dictionary<string, FromBrowserMethodDelegate> _methods;

        private readonly IModAPI _modAPI;
        private readonly ILoggingHandler _loggingHandler;
        private readonly CustomLobbyMenuSyncHandler _customLobbyMenuSyncHandler;

        public RemoteBrowserEventsHandler(UserpanelHandler userpanelHandler, LobbiesHandler lobbiesHandler, InvitationsHandler invitationsHandler, MapsLoadingHandler mapsLoadingHandler,
            ILoggingHandler loggingHandler, CustomLobbyMenuSyncHandler customLobbyMenuSyncHandler, MapCreatorHandler mapCreatorHandler, MapCreatorHandler _mapCreatorHandler,
            MapFavouritesHandler mapFavouritesHandler, IModAPI modAPI)
        {
            _modAPI = modAPI;
            _loggingHandler = loggingHandler;
            _customLobbyMenuSyncHandler = customLobbyMenuSyncHandler;

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
                [ToServerEvent.SaveMapCreatorData] = _mapCreatorHandler.Save,
                [ToServerEvent.SendMapCreatorData] = _mapCreatorHandler.Create,
                [ToServerEvent.SaveSettings] = userpanelHandler.SettingsNormalHandler.SaveSettings,
                [ToServerEvent.SendApplication] = userpanelHandler.ApplicationUserHandler.CreateApplication,
                [ToServerEvent.SetSupportRequestClosed] = userpanelHandler.SupportRequestHandler.SetSupportRequestClosed,
                [ToServerEvent.SendSupportRequest] = userpanelHandler.SupportRequestHandler.SendRequest,
                [ToServerEvent.SendSupportRequestMessage] = userpanelHandler.SupportRequestHandler.SendMessage,
                [ToServerEvent.ToggleMapFavouriteState] = mapFavouritesHandler.ToggleMapFavouriteState,
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
                [ToServerEvent.MapCreatorSyncCurrentMapToServer] = mapCreatorHandler.SyncCurrentMapToClient

            };
        }

        public async void OnFromBrowserEvent(ITDSPlayer player, params object[] args)
        {
            try
            {
                object? ret = null;

                string eventName = (string)args[0];
                if (_asyncMethods.ContainsKey(eventName))
                {
                    ret = await _asyncMethods[eventName](player, args.Skip(1).ToArray());
                }
                else if (_maybeAsyncMethods.ContainsKey(eventName))
                {
                    ret = await _maybeAsyncMethods[eventName](player, args.Skip(1).ToArray());
                }

                _modAPI.Thread.RunInMainThread(() =>
                {
                    if (_methods.ContainsKey(eventName))
                    {
                        ret = _methods[eventName](player, args.Skip(1).ToArray());
                    }

                    if (ret != null)
                    {
                        player.SendEvent(ToClientEvent.FromBrowserEventReturn, eventName, ret);
                    }
                });
                
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex.GetBaseException().Message + "\n"
                    + String.Join('\n', args.Select(a => Convert.ToString(a)?.Substring(0, Math.Min(Convert.ToString(a)?.Length ?? 0, 20)) ?? "-")),
                    ex.StackTrace ?? Environment.StackTrace, player);
            }
        }

        private object? BuyMap(ITDSPlayer player, object[] args)
        {
            if (player.Lobby is null)
                return null;
            if (!(player.Lobby is Arena arena))
                return null;
            int? mapId;
            if ((mapId = Utils.GetInt(args[0])) == null)
                return null;

            arena.BuyMap(player, mapId.Value);
            return null;
        }

        private object? MapCreatorSyncData(ITDSPlayer player, object[] args)
        {
            if (player.Lobby is null)
                return null;
            if (!(player.Lobby is MapCreateLobby lobby))
                return null;
            var infoType = (MapCreatorInfoType)Convert.ToInt32(args[0]);
            var data = args[1];

            lobby.SyncMapInfoChange(infoType, data);
            return null;
        }

        private object? MapVote(ITDSPlayer player, object[] args)
        {
            if (!(player.Lobby is Arena arena))
                return null;

            if (args.Length == 0)
                return null;

            int? mapId;
            if ((mapId = Utils.GetInt(args[0])) == null)
                return null;

            arena.MapVote(player, mapId.Value);
            return null;
        }

        private object? GiveVehicle(ITDSPlayer player, object[] args)
        {
            if (player.Lobby is null || !(player.Lobby is MapCreateLobby lobby))
                return null;

            if (args.Length == 0)
                return null;

            if (!Enum.TryParse(args[0].ToString(), out FreeroamVehicleType vehType))
                return null;

            lobby.GiveVehicle(player, vehType);
            return null;
        }

        private object? JoinedCustomLobbiesMenu(ITDSPlayer player, object[] args)
        {
            _customLobbyMenuSyncHandler.AddPlayer(player);
            return null;
        }

        private object? LeftCustomLobbiesMenu(ITDSPlayer player, object[] args)
        {
            _customLobbyMenuSyncHandler.RemovePlayer(player);
            return null;
        }
    }
}
