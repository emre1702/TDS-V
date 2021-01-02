using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Helper;

namespace TDS.Server.Handler.Maps.MapCreatorRewards
{
    internal class MapCreatorRewardsGiveOnlineHandler
    {
        internal void GiveReward(ITDSPlayer onlinePlayer, IRoundFightLobby lobby, int reward)
        {
            onlinePlayer.Money += reward;

            lobby.Notifications.Send(lang => string.Format(lang.MAP_CREATOR_REWARD_INFO, lobby.CurrentMap.BrowserSyncedData.CreatorName, reward));

            if (onlinePlayer.Lobby != lobby)
                onlinePlayer.SendNotification(string.Format(onlinePlayer.Language.YOU_GOT_MAP_CREATOR_REWARD, lobby.CurrentMap.BrowserSyncedData.Name, reward));
        }
    }
}