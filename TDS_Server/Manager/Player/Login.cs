namespace TDS_Server.Manager.Player
{
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS_Server.Default;
    using TDS_Server.Entity;
    using TDS_Server.Instance.Language;
    using TDS_Server.Instance.Player;
    using TDS_Server.Manager.Utility;
    using TDS_Common.Default;

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

                TDSPlayer character = player.GetChar();
                dbcontext.Attach(entity.Playerstats);
                character.Entity = entity;

                if (entity.AdminLvl > 0)
                    AdminsManager.SetOnline(player);

                player.Team = 1;        // To be able to use custom damagesystem
                entity.Playerstats.LoggedIn = true;

                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.RegisterLoginSuccessful, entity.AdminLvl);
                //MainMenu.Join(character);

                await dbcontext.SaveChangesAsync();

                //Map.SendPlayerHisRatings(character);
                //Gang.CheckPlayerGang(character);

                NAPI.ClientEvent.TriggerClientEvent(player, DToClientEvent.LoadSettings, SettingsManager.SyncedSettings);

                Logs.Rest.Log(Enum.ELogType.Login, player, true);    
            }
        }
    }

}
