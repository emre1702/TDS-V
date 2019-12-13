﻿using GTANetworkAPI;
using System;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity.Player;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.Lobby;
using TDS_Server.Manager.Utility;
using TDS_Common.Enum;

namespace TDS_Server.Manager.EventManager
{
    partial class EventsHandler
    {
        public delegate Task<object?> FromBrowserAsyncMethodDelegate(TDSPlayer player, params object[] args);
        public delegate object? FromBrowserMethodDelegate(TDSPlayer player, params object[] args);

        private static readonly Dictionary<string, FromBrowserAsyncMethodDelegate> _AsyncMethods = new Dictionary<string, FromBrowserAsyncMethodDelegate>
        {
            [DToServerEvent.SendApplicationInvite] = Userpanel.ApplicationsAdmin.SendInvitation,
            [DToServerEvent.AnswerToOfflineMessage] = Userpanel.OfflineMessages.Answer,
            [DToServerEvent.SendOfflineMessage] = Userpanel.OfflineMessages.Send,
            [DToServerEvent.DeleteOfflineMessage] = Userpanel.OfflineMessages.Delete,
            [DToServerEvent.SaveSpecialSettingsChange] = Userpanel.SettingsSpecial.SetData
        };
        private static readonly Dictionary<string, FromBrowserMethodDelegate> _methods = new Dictionary<string, FromBrowserMethodDelegate>
        {
            [DToServerEvent.BuyMap] = BuyMap,
            [DToServerEvent.MapCreatorSyncData] = MapCreatorSyncData,
            [DToServerEvent.AcceptInvitation] = InvitationManager.AcceptInvitation,
            [DToServerEvent.RejectInvitation] = InvitationManager.RejectInvitation
        };

        [RemoteEvent(DToServerEvent.FromBrowserEvent)]
        public static async void OnFromBrowserEvent(Client client, string eventName, params object[] args)
        {
            try
            {
                object? ret = null;
                TDSPlayer player = client.GetChar();
                if (!player.LoggedIn)
                    return;

                if (_AsyncMethods.ContainsKey(eventName))
                {
                    ret = await _AsyncMethods[eventName](player, args);
                } 
                else if (_methods.ContainsKey(eventName))
                {
                    ret = _methods[eventName](player, args);
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
        public async void PlayerSaveSettings(Client client, string json)
        {
            try
            {
                TDSPlayer player = client.GetChar();
                if (!player.LoggedIn || player.Entity is null)
                    return;

                var obj = Serializer.FromBrowser<PlayerSettings>(json);
                await player.ExecuteForDB((dbContext) =>
                {
                    dbContext.Entry(player.Entity.PlayerSettings).CurrentValues.SetValues(obj);
                });
                await player.SaveData();

                player.LoadTimeZone();

                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SyncSettings, json);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log("Error in PlayerSaveSettings: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, client);
            }            
        }

        [RemoteEvent(DToServerEvent.GetSupportRequestData)]
        public async void GetSupportRequestDataMethod(Client client, int requestId)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            await Userpanel.SupportRequest.GetSupportRequestData(player, requestId);
        }

        [RemoteEvent(DToServerEvent.SetSupportRequestClosed)]
        public async void SetSupportRequestClosedMethod(Client client, int requestId, bool closed)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            await Userpanel.SupportRequest.SetSupportRequestClosed(player, requestId, closed);
        }

        [RemoteEvent(DToServerEvent.LeftSupportRequestsList)]
        public void LeftSupportRequestsListMethod(Client client)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            Userpanel.SupportRequest.LeftSupportRequestsList(player);
        }

        [RemoteEvent(DToServerEvent.LeftSupportRequest)]
        public void LeftSupportRequestMethod(Client client, int requestId)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            Userpanel.SupportRequest.LeftSupportRequest(player, requestId);
        }

        [RemoteEvent(DToServerEvent.SendSupportRequest)]
        public async void SendSupportRequestMethod(Client client, string json)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            await Userpanel.SupportRequest.SendRequest(player, json);
        }

        [RemoteEvent(DToServerEvent.SendSupportRequestMessage)]
        public async void SendSupportRequestMessageMethod(Client client, int requestId, string message)
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            await Userpanel.SupportRequest.SendMessage(player, requestId, message);
        }

        private static object? BuyMap(TDSPlayer player, params object[] args)
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

        private static object? MapCreatorSyncData(TDSPlayer player, params object[] args)
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
