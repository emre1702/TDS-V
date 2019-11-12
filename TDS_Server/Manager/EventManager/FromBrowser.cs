using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using Newtonsoft.Json;
using TDS_Common.Default;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.EventManager
{
    partial class EventsHandler
    {

        [RemoteEvent(DToServerEvent.SaveSettings)]
        public async void PlayerSaveSettings(Client client, string json)
        {
            try
            {
                TDSPlayer player = client.GetChar();
                if (!player.LoggedIn || player.Entity is null)
                    return;

                var obj = JsonConvert.DeserializeObject<PlayerSettings>(json);
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

        [RemoteEvent(DToServerEvent.SendApplicationInvite)]
        public async void SendApplicationInviteMethod(Client client, int applicationId, string message) 
        {
            TDSPlayer player = client.GetChar();
            if (!player.LoggedIn)
                return;

            await Userpanel.ApplicationsAdmin.SendInvitation(player, applicationId, message);
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
    }
}
