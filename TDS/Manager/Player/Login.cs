namespace TDS.Manager.Player
{
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS.Default;
    using TDS.Entity;
    using TDS.Instance.Language;
    using TDS.Manager.Utility;

    static class Login
    {

        public static async void LoginPlayer(Client player, uint id, string password)
        {
            using (var dbcontext = new TDSNewContext())
            {
                Players entity = await dbcontext.Players
                                        .Include(p => p.Playersettings)
                                        .Include(p => p.OfflinemessagesTarget)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(p => p.Id == id);
                if (entity == null)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, LangUtils.GetLang(typeof(English)).ACCOUNT_DOESNT_EXIST);
                    return;
                }

                if (Utils.ToSHA512(password) != entity.Password)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, player.GetLang().WRONG_PASSWORD);
                    return;
                }

                dbcontext.Attach(entity.Playerstats);

                player.GiveMoney(0);        // to update at clientside

                if (entity.AdminLvl > 0)
                    AdminsManager.SetOnline(player);

                player.Team = 1;        // To be able to use custom damagesystem
                entity.Playerstats.LoggedIn = true;

                NAPI.ClientEvent.TriggerClientEvent(player, DCustomEvents.RegisterLoginSuccessful, entity.AdminLvl);
                //MainMenu.Join(character);

                await dbcontext.SaveChangesAsync();

                //Map.SendPlayerHisRatings(character);
                //Gang.CheckPlayerGang(character);

                Logs.Rest.Log(Enum.ELogType.Login, player, true);    
            }
        }
    }

}
