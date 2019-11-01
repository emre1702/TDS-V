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
    }
}
