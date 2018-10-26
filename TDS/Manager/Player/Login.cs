namespace TDS.Manager.Player
{
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS.Default;
    using TDS.Entity;
    using TDS.Manager.Language;
    using TDS.Manager.Utility;

    static class Login
    {

        public static async void LoginPlayer(Client player, uint id, string password)
        {
            using (var dbcontext = new TDSNewContext())
            {
                Players character = await dbcontext.Players
                                        .Include(p => p.Playersettings)
                                        .Include(p => p.OfflinemessagesTarget)
                                        .FirstOrDefaultAsync(p => p.Id == id);
                if (character == null)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, LangUtils.GetLang(typeof(English)).ACCOUNT_DOESNT_EXIST);
                    return;
                }

                if (Utils.ToSHA512(password) != character.Password)
                {
                    NAPI.Chat.SendChatMessageToPlayer(player, character.GetLang().WRONG_PASSWORD);
                    return;
                }

                player.GiveMoney(0);        // to update at clientside

                if (character.AdminLvl > 0)
                    Admin.SetOnline(player);

                player.Team = 1;        // To be able to use custom damagesystem
                character.LoggedIn = true;

                NAPI.ClientEvent.TriggerClientEvent(player, DCustomEvents.RegisterLoginSuccessful, character.AdminLvl);
                //MainMenu.Join(character);

                dbcontext.Players.Attach(character);
                dbcontext.Entry(character).Property(p => p.LoggedIn).IsModified = true;
                await dbcontext.SaveChangesAsync();

                //Map.SendPlayerHisRatings(character);
                //Gang.CheckPlayerGang(character);

                Logs.Rest.Log(Enum.ELogType.Login, player, true);    
            }
        }
    }

}
