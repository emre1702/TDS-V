﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
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
                if (!player.LoggedIn || player.Entity == null)
                    return;

                var obj = JsonSerializer.Deserialize<PlayerSettings>(json);
                player.Entity.PlayerSettings = obj;
                player.DbContext.Entry(obj).State = EntityState.Modified;
                await player.SaveData();

                NAPI.ClientEvent.TriggerClientEvent(client, DToClientEvent.SyncSettings, json);
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log("Error in PlayerSaveSettings: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, client);
            }

            
        }
    }
}
