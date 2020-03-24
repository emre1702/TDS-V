/*using GTANetworkAPI;
using System;
using TDS_Common.Default;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Database.Entity.Player;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Manager.Utility;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Enums.Challenge;
using BonusBotConnector_Client.Requests;
using TDS_Server.Manager.Userpanel;
using TDS_Shared.Default;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Core.Manager.EventManager
{
    partial class EventsHandler
    {
        public delegate Task<object?> FromBrowserAsyncMethodDelegate(ITDSPlayer player, object[] args);
        public delegate ValueTask<object?> FromBrowserMaybeAsyncMethodDelegate(ITDSPlayer player, object[] args);
        public delegate object? FromBrowserMethodDelegate(ITDSPlayer player, object[] args);

        private static readonly Dictionary<string, FromBrowserAsyncMethodDelegate> _asyncMethods = new Dictionary<string, FromBrowserAsyncMethodDelegate>
        {
            [ToServerEvent.SendApplicationInvite] = Userpanel.ApplicationsAdmin.SendInvitation,
            [ToServerEvent.AnswerToOfflineMessage] = Userpanel.OfflineMessages.Answer,
            [ToServerEvent.SendOfflineMessage] = Userpanel.OfflineMessages.Send,
            [ToServerEvent.DeleteOfflineMessage] = Userpanel.OfflineMessages.Delete,
            [ToServerEvent.SaveSpecialSettingsChange] = Userpanel.SettingsSpecial.SetData
        };
        private static readonly Dictionary<string, FromBrowserMaybeAsyncMethodDelegate> _maybeAsyncMethods = new Dictionary<string, FromBrowserMaybeAsyncMethodDelegate>
        {
            [ToServerEvent.LoadDatasForCustomLobby] = LobbyManager.LoadDatas
        };
        private static readonly Dictionary<string, FromBrowserMethodDelegate> _methods = new Dictionary<string, FromBrowserMethodDelegate>
        {
            [ToServerEvent.BuyMap] = BuyMap,
            [ToServerEvent.MapCreatorSyncData] = MapCreatorSyncData,
            [ToServerEvent.AcceptInvitation] = InvitationManager.AcceptInvitation,
            [ToServerEvent.RejectInvitation] = InvitationManager.RejectInvitation,
            [ToServerEvent.LoadAllMapsForCustomLobby] = Maps.MapsLoader.GetAllMapsForCustomLobby,
        };

        [RemoteEvent(DToServerEvent.FromBrowserEvent)]
        public static async void OnFromBrowserEvent(ITDSPlayer player, params object[] args)
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
                    NAPI.ClientEvent.TriggerClientEvent(client, ToClientEvent.FromBrowserEventReturn, eventName, ret);
                }
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.GetBaseException().Message + "\n" 
                    + String.Join('\n', args.Select(a => Convert.ToString(a)?.Substring(0, Math.Min(Convert.ToString(a)?.Length ?? 0, 20)) ?? "-")),
                    ex.StackTrace ?? Environment.StackTrace, client);
            }
        }

        [RemoteEvent(DToServerEvent.SaveSettings)]
        public void PlayerSaveSettings(Player client, string json)
        {
            try
            {
                TDSPlayer player = client.GetChar();
                if (!player.LoggedIn || player.Entity is null)
                    return;

                SettingsNormal.SaveSettings(player, json);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log("Error in PlayerSaveSettings: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, client);
            }            
        }

        [RemoteEvent(DToServerEvent.GetSupportRequestData)]
        public async void GetSupportRequestDataMethod(Player client, int requestId)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            await Userpanel.SupportRequest.GetSupportRequestData(player, requestId);
        }

        [RemoteEvent(DToServerEvent.SetSupportRequestClosed)]
        public async void SetSupportRequestClosedMethod(Player client, int requestId, bool closed)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            await Userpanel.SupportRequest.SetSupportRequestClosed(player, requestId, closed);
        }

        [RemoteEvent(DToServerEvent.LeftSupportRequestsList)]
        public void LeftSupportRequestsListMethod(Player client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            Userpanel.SupportRequest.LeftSupportRequestsList(player);
        }

        [RemoteEvent(DToServerEvent.LeftSupportRequest)]
        public void LeftSupportRequestMethod(Player client, int requestId)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            Userpanel.SupportRequest.LeftSupportRequest(player, requestId);
        }

        [RemoteEvent(DToServerEvent.SendSupportRequest)]
        public async void SendSupportRequestMethod(Player client, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            await Userpanel.SupportRequest.SendRequest(player, json);
        }

        [RemoteEvent(DToServerEvent.SendSupportRequestMessage)]
        public async void SendSupportRequestMessageMethod(Player client, int requestId, string message)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            await Userpanel.SupportRequest.SendMessage(player, requestId, message);
        }

        private static object? BuyMap(TDSPlayer player, object[] args)
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

        private static object? MapCreatorSyncData(TDSPlayer player, object[] args)
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
*/
