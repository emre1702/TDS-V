using MimeKit;
using System;
using System.Threading.Tasks;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.MailSystem;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity.Player;
using TDS.Server.MailSystem.Datas;

namespace TDS.Server.MailSystem
{
    public class MailSender : IMailSender
    {
        private readonly ILoggingHandler _loggingHandler;
        private readonly SubjectProvider _subjectProvider;
        private readonly BodyProvider _bodyProvider;

        public MailSender(ILoggingHandler loggingHandler)
        {
            _loggingHandler = loggingHandler;
            _subjectProvider = new SubjectProvider();
            _bodyProvider = new BodyProvider();
        }

        public async Task<SmtpClientResponse> SendPasswordResetMail(Players playerEntity, string newPassword, ILanguage language)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(playerEntity.Email))
                    return new SmtpClientResponse { ErrorMessage = language.NO_EMAIL_ADDRESS_HAS_BEEN_SET };
                if (!InternetAddress.TryParse(playerEntity.Email, out InternetAddress targetAddress))
                    return new SmtpClientResponse { ErrorMessage = language.EMAIL_ADDRESS_FOR_ACCOUNT_IS_INVALID };

                var options = new TDSMailOptions
                {
                    Subject = _subjectProvider.Get(MailType.PasswordReset),
                    Body = _bodyProvider.Get(MailType.PasswordReset, newPassword),
                    ReceiverAddress = playerEntity.Email
                };
                var mail = new TDSMail(options);

                using var client = new TDSSmtpClient(_loggingHandler, language);
                await client.ConnectAsync("localhost", 0, MailKit.Security.SecureSocketOptions.None).ConfigureAwait(false);
                await client.SendAsync(mail).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
                var possibleErrorMessage = await client.GetErrorMessage(language).ConfigureAwait(false);
                return new SmtpClientResponse { ErrorMessage = possibleErrorMessage };
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
                return new SmtpClientResponse { ErrorMessage = language.ERROR_INFO };
            }
        }
    }
}
