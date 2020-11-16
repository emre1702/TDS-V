using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.MailSystem;
using TDS.Server.Data.Utility;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Utility;
using TDS.Shared.Default;

namespace TDS.Server.Handler.Account
{
    public class ResetPasswordHandler
    {
        private readonly IMailSender _mailSender;
        private readonly ILoggingHandler _loggingHandler;

        public ResetPasswordHandler(IMailSender mailSender, ILoggingHandler loggingHandler)
        {
            _mailSender = mailSender;
            _loggingHandler = loggingHandler;

            NAPI.ClientEvent.Register<ITDSPlayer, string, string>(ToServerEvent.ResetPassword, this, ResetPassword);
        }

        private async void ResetPassword(ITDSPlayer player, string username, string email)
        {
            try
            {
                var isEmailCorrect = await player.Database.ExecuteForDBAsync(async dbContext =>
                    await dbContext.Players.AnyAsync(p => p.Name == username && p.Email == email).ConfigureAwait(false)).ConfigureAwait(false);
                if (!isEmailCorrect)
                {
                    NAPI.Task.RunSafe(() =>
                        player.SendAlert(player.Language.EMAIL_ADDRESS_FOR_ACCOUNT_IS_INVALID));
                    return;
                }

                var newPassword = GeneratePassword();
                var newPasswordHashed = Utils.HashPasswordServer(SharedUtils.HashPWClient(newPassword));

                var playerEntity = await player.Database
                    .ExecuteForDBAsync(async dbContext => await dbContext
                        .Players
                        .FirstOrDefaultAsync(p => p.Name == username)
                        .ConfigureAwait(false))
                    .ConfigureAwait(false);
                playerEntity.Password = newPasswordHashed;
                await player.Database.ExecuteForDBAsync(async dbContext => await dbContext.SaveChangesAsync().ConfigureAwait(false)).ConfigureAwait(false);
                _loggingHandler.LogRest(LogType.ResetPassword, player, true);

                var response = await _mailSender.SendPasswordResetMail(playerEntity, newPassword, player.Language).ConfigureAwait(false);
                if (response.Worked)
                    player.SendAlert(player.Language.PASSWORD_HAS_BEEN_RESET_EMAIL_SENT);
                else if (response.ErrorMessage is { })
                    player.SendAlert(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
                player.SendAlert(player.Language.ERROR_INFO);
            }
        }

        private string GeneratePassword()
        {
            var amountChars = SharedUtils.Rnd.Next(15, 25);
            var strBuilder = new StringBuilder();
            for (int i = 0; i < amountChars; ++i)
                strBuilder.Append((char)SharedUtils.Rnd.Next(33, 123));
            return strBuilder.ToString();
        }
    }
}
