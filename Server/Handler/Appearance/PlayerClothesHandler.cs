using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Database.Entity.Player.Character.Clothes;
using TDS.Server.Handler.Events;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Appearance
{
    public class PlayerClothesHandler
    {
        public PlayerClothesHandler(RemoteBrowserEventsHandler remoteBrowserEventsHandler)
        {
            remoteBrowserEventsHandler.AddAsyncEvent(ToServerEvent.SaveClothesData, Save);
        }

        private async Task<object?> Save(ITDSPlayer player, ArraySegment<object> args)
        {
            try
            {
                if (player.Entity is null)
                    return "ErrorInfo";
                var newConfig = Serializer.FromBrowser<PlayerClothesData>((string)args[0]);
                await player.Database.ExecuteForDBAsyncUnsafe(async dbContext =>
                {
                    if (player.Entity.ClothesDatas is { })
                        dbContext.Entry(player.Entity.ClothesDatas).State = EntityState.Detached;
                    player.Entity.ClothesDatas = newConfig;
                });

                return "";
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex, player);
                return "ErrorInfo";
            }
        }
    }
}