﻿using BonusBotConnector_Client.Requests;
using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TDS_Common.Default;
using TDS_Common.Enum.Challenge;
using TDS_Common.Manager.Utility;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Manager.Userpanel
{
    static class SettingsNormal
    {
        private static Dictionary<ulong, int> _playerIdWaitingForDiscordUserIdConfirm = new Dictionary<ulong, int>();

        public static async void SaveSettings(TDSPlayer player, string json)
        {
            var obj = Serializer.FromBrowser<PlayerSettings>(json);

            var newDiscordUserId = obj.DiscordUserId;
            obj.DiscordUserId = player.Entity!.PlayerSettings.DiscordUserId;
            await player.ExecuteForDBAsync(async (dbContext) =>
            {
                dbContext.Entry(player.Entity.PlayerSettings).CurrentValues.SetValues(obj);
                await dbContext.SaveChangesAsync();
            });

            player.LoadTimezone();
            player.AddToChallenge(EChallengeType.ChangeSettings);

            NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.SyncSettings, json);

            if (newDiscordUserId != player.Entity.PlayerSettings.DiscordUserId)
            {
                PrivateChat.SendMessage(string.Format(player.Language.DISCORD_IDENTITY_CHANGED_BONUSBOT_INFO, player.DisplayName), newDiscordUserId, (reply) =>
                {
                    if (string.IsNullOrEmpty(reply.ErrorMessage))
                        return;

                    player.SendMessage(string.Format(player.Language.DISCORD_IDENTITY_SAVE_FAILED, reply.ErrorMessage));
                });
            }
        }

        public static string ConfirmDiscordUserId(ulong discordUserId)
        {
            if (!_playerIdWaitingForDiscordUserIdConfirm.TryGetValue(discordUserId, out int userId))
                return "This discord user id is not waiting for a confirmation.";

            try
            {
                var player = PlayerManager.PlayerManager.GetPlayerByID(userId);
                if (player is null)
                    SaveDiscordUserId(userId, discordUserId);
                else 
                    SaveDiscordUserId(player, discordUserId);
                return "Discord Id confirmation was successful";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.GetBaseException().Message;
            }
        } 

        private static async void SaveDiscordUserId(int userId, ulong discordUserId)
        {
            using var dbContext = new TDSDbContext();

            var settings = await dbContext.PlayerSettings.FirstOrDefaultAsync(s => s.PlayerId == userId);
            if (settings is null)
                throw new Exception("Your player settings do not exist?!");
            settings.DiscordUserId = discordUserId;
            await dbContext.SaveChangesAsync();
        }

        private static async void SaveDiscordUserId(TDSPlayer player, ulong discordUserId)
        {
            player.Entity!.PlayerSettings.DiscordUserId = discordUserId;
            await player.SaveData(true);
        }
    }
}