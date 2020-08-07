using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.GangSystem;
using TDS_Server.Handler.Maps;
using TDS_Server.Handler.Player;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Events
{
    public class RemoteBrowserEventsHandler
    {
        #region Private Fields

        private readonly Dictionary<string, FromBrowserAsyncMethodDelegate> _asyncMethods;

        private readonly CustomLobbyMenuSyncHandler _customLobbyMenuSyncHandler;

        private readonly ILoggingHandler _loggingHandler;

        private readonly Dictionary<string, FromBrowserMaybeAsyncMethodDelegate> _maybeAsyncMethods;

        private readonly Dictionary<string, FromBrowserMethodDelegate> _methods;

        private readonly IModAPI _modAPI;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;
        private readonly EventsHandler _eventsHandler;

        #endregion Private Fields

        #region Public Constructors

        public RemoteBrowserEventsHandler(IUserpanelHandler userpanelHandler, LobbiesHandler lobbiesHandler, InvitationsHandler invitationsHandler, MapsLoadingHandler mapsLoadingHandler,
            ILoggingHandler loggingHandler, CustomLobbyMenuSyncHandler customLobbyMenuSyncHandler, MapCreatorHandler mapCreatorHandler,
            MapFavouritesHandler mapFavouritesHandler, IModAPI modAPI, PlayerCharHandler playerCharHandler, ITDSPlayerHandler tdsPlayerHandler,
            GangWindowHandler gangWindowHandler, EventsHandler eventsHandler)
        {
            _modAPI = modAPI;
            _loggingHandler = loggingHandler;
            _customLobbyMenuSyncHandler = customLobbyMenuSyncHandler;
            _tdsPlayerHandler = tdsPlayerHandler;
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
                [ToServerEvent.LoadGangWindowData] = gangWindowHandler.OnLoadGangWindowData
            };

            modAPI.ClientEvent.Add<IPlayer, object[]>(ToServerEvent.FromBrowserEvent, this, OnFromBrowserEvent);
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate Task<object?> FromBrowserAsyncMethodDelegate(ITDSPlayer player, ArraySegment<object> args);

        public delegate ValueTask<object?> FromBrowserMaybeAsyncMethodDelegate(ITDSPlayer player, ArraySegment<object> args);

        public delegate object? FromBrowserMethodDelegate(ITDSPlayer player, ref ArraySegment<object> args);

        #endregion Public Delegates

        #region Public Methods

        public async void OnFromBrowserEvent(IPlayer modPlayer, params object[] args)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
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

                _modAPI.Thread.QueueIntoMainThread(() =>
                {
                    if (_methods.ContainsKey(eventName))
                    {
                        ret = _methods[eventName](player, ref argsWithoutEventName);
                    }

                    if (ret != null)
                    {
                        player.SendEvent(ToClientEvent.FromBrowserEventReturn, eventName, ret);
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

        #endregion Public Methods

        #region Private Methods

        private object? BuyMap(ITDSPlayer player, ref ArraySegment<object> args)
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

        private object? GiveVehicle(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (player.Lobby is null || !(player.Lobby is MapCreateLobby lobby))
                return null;

            if (args.Count == 0)
                return null;

            if (!Enum.TryParse(args[0].ToString(), out FreeroamVehicleType vehType))
                return null;

            lobby.GiveVehicle(player, vehType);
            return null;
        }

        private object? JoinedCustomLobbiesMenu(ITDSPlayer player, ref ArraySegment<object> args)
        {
            _customLobbyMenuSyncHandler.AddPlayer(player);
            _eventsHandler.OnCustomLobbyMenuJoin(player);
            return null;
        }

        private object? LeftCustomLobbiesMenu(ITDSPlayer player, ref ArraySegment<object> args)
        {
            _customLobbyMenuSyncHandler.RemovePlayer(player);
            return null;
        }

        private object? MapCreatorSyncData(ITDSPlayer player, ref ArraySegment<object> args)
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

        private object? MapVote(ITDSPlayer player, ref ArraySegment<object> args)
        {
            if (!(player.Lobby is Arena arena))
                return null;

            if (args.Count == 0)
                return null;

            int? mapId;
            if ((mapId = Utils.GetInt(args[0])) == null)
                return null;

            arena.MapVote(player, mapId.Value);
            return null;
        }

        #endregion Private Methods
    }
}
