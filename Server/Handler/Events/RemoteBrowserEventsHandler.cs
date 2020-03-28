using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Utility;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Maps;
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

        private readonly UserpanelHandler _userpanelHandler;
        private readonly ILoggingHandler _loggingHandler;

        public RemoteBrowserEventsHandler(UserpanelHandler userpanelHandler, LobbiesHandler lobbiesHandler, InvitationsHandler invitationsHandler, MapsLoadingHandler mapsLoadingHandler,
            ILoggingHandler loggingHandler)
        {
            _userpanelHandler = userpanelHandler;
            _loggingHandler = loggingHandler;

            _asyncMethods = new Dictionary<string, FromBrowserAsyncMethodDelegate>
            {
                [ToServerEvent.SendApplicationInvite] = userpanelHandler.ApplicationsAdminHandler.SendInvitation,
                [ToServerEvent.AnswerToOfflineMessage] = userpanelHandler.OfflineMessagesHandler.Answer,
                [ToServerEvent.SendOfflineMessage] = userpanelHandler.OfflineMessagesHandler.Send,
                [ToServerEvent.DeleteOfflineMessage] = userpanelHandler.OfflineMessagesHandler.Delete,
                [ToServerEvent.SaveSpecialSettingsChange] = userpanelHandler.SettingsSpecialHandler.SetData
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
                else if (_asyncMethods.ContainsKey(eventName))
                {
                    ret = await _maybeAsyncMethods[eventName](player, args.Skip(1).ToArray());
                }
                else if (_methods.ContainsKey(eventName))
                {
                    ret = _methods[eventName](player, args.Skip(1).ToArray());
                }

                if (ret != null)
                {
                    player.SendEvent(ToClientEvent.FromBrowserEventReturn, eventName, ret); 
                }
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex.GetBaseException().Message + "\n"
                    + String.Join('\n', args.Select(a => Convert.ToString(a)?.Substring(0, Math.Min(Convert.ToString(a)?.Length ?? 0, 20)) ?? "-")),
                    ex.StackTrace ?? Environment.StackTrace, player);
            }
        }


        public void OnSaveSettings(ITDSPlayer player, string json)
        {
            try
            {
                _userpanelHandler.SettingsNormalHandler.SaveSettings(player, json);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError("Error in PlayerSaveSettings: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, player);
            }
        }

        public async void OnGetSupportRequestData(ITDSPlayer player, int requestId)
        {
            await _userpanelHandler.SupportRequestHandler.GetSupportRequestData(player, requestId);
        }

        public async void OnSetSupportRequestClosed(ITDSPlayer player, int requestId, bool closed)
        {
            await _userpanelHandler.SupportRequestHandler.SetSupportRequestClosed(player, requestId, closed);
        }

        public void OnLeftSupportRequestsList(ITDSPlayer player)
        {
            _userpanelHandler.SupportRequestHandler.LeftSupportRequestsList(player);
        }

        public void OnLeftSupportRequest(ITDSPlayer player, int requestId)
        {
            _userpanelHandler.SupportRequestHandler.LeftSupportRequest(player, requestId);
        }

        public async void OnSendSupportRequest(ITDSPlayer player, string json)
        {
            await _userpanelHandler.SupportRequestHandler.SendRequest(player, json);
        }

        public async void OnSendSupportRequestMessage(ITDSPlayer player, int requestId, string message)
        {
            await _userpanelHandler.SupportRequestHandler.SendMessage(player, requestId, message);
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
    }
}
