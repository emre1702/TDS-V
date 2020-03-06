using GTANetworkAPI;
using System;
using TDS_Common.Default;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.PlayerManager;
using TDS_Server_DB.Entity.Player;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.LobbyInstances;
using TDS_Server.Manager.Utility;
using TDS_Common.Enum;
using TDS_Common.Enum.Challenge;
using BonusBotConnector_Client.Requests;
using TDS_Server.Manager.Userpanel;

namespace TDS_Server.Manager.EventManager
{
    partial class EventsHandler
    {
        public delegate Task<object?> FromBrowserAsyncMethodDelegate(TDSPlayer player, object[] args);
        public delegate ValueTask<object?> FromBrowserMaybeAsyncMethodDelegate(TDSPlayer player, object[] args);
        public delegate object? FromBrowserMethodDelegate(TDSPlayer player, object[] args);

        private static readonly Dictionary<string, FromBrowserAsyncMethodDelegate> _asyncMethods = new Dictionary<string, FromBrowserAsyncMethodDelegate>
        {
            [DToServerEvent.SendApplicationInvite] = Userpanel.ApplicationsAdmin.SendInvitation,
            [DToServerEvent.AnswerToOfflineMessage] = Userpanel.OfflineMessages.Answer,
            [DToServerEvent.SendOfflineMessage] = Userpanel.OfflineMessages.Send,
            [DToServerEvent.DeleteOfflineMessage] = Userpanel.OfflineMessages.Delete,
            [DToServerEvent.SaveSpecialSettingsChange] = Userpanel.SettingsSpecial.SetData
        };
        private static readonly Dictionary<string, FromBrowserMaybeAsyncMethodDelegate> _maybeAsyncMethods = new Dictionary<string, FromBrowserMaybeAsyncMethodDelegate>
        {
            
        };
        private static readonly Dictionary<string, FromBrowserMethodDelegate> _methods = new Dictionary<string, FromBrowserMethodDelegate>
        {
            [DToServerEvent.BuyMap] = BuyMap,
            [DToServerEvent.MapCreatorSyncData] = MapCreatorSyncData,
            [DToServerEvent.AcceptInvitation] = InvitationManager.AcceptInvitation,
            [DToServerEvent.RejectInvitation] = InvitationManager.RejectInvitation,
            [DToServerEvent.LoadAllMapsForCustomLobby] = Maps.MapsLoader.GetAllMapsForCustomLobby,
        };

        [RemoteEvent(DToServerEvent.FromBrowserEvent)]
        public static async void OnFromBrowserEvent(Player client, params object[] args)
        {
            try
            {
                object? ret = null;
                TDSPlayer player = client.GetChar();
                if (!player.LoggedIn)
                    return;

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
                    NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.FromBrowserEventReturn, eventName, ret);
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
            if (player.CurrentLobby is null)
                return null;
            if (!(player.CurrentLobby is Arena arena))
                return null;
            int? mapId;
            if ((mapId = Utils.GetInt(args[0])) == null)
                return null;

            arena.BuyMap(player, mapId.Value);
            return null;
        }

        private static object? MapCreatorSyncData(TDSPlayer player, object[] args)
        {
            if (player.CurrentLobby is null)
                return null;
            if (!(player.CurrentLobby is MapCreateLobby lobby))
                return null;
            var infoType = (EMapCreatorInfoType)Convert.ToInt32(args[0]);
            var data = args[1];

            lobby.SyncMapInfoChange(infoType, data);
            return null;
        }
    }
}
