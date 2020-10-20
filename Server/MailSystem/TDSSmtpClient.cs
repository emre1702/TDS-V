using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.MailSystem
{
    internal class TDSSmtpClient : SmtpClient
    {
        private TaskCompletionSource<string?>? _sendCompletionSource;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ILanguage _userLanguage;

        public TDSSmtpClient(ILoggingHandler loggingHandler, ILanguage userLanguage)
        {
            _loggingHandler = loggingHandler;
            _userLanguage = userLanguage;
        }

        internal async Task<string?> GetErrorMessage(ILanguage language)
        {
            if (_sendCompletionSource is null)
            {
                _loggingHandler.LogError("TDSSmtpClient GetErrorMessage has been used but _sendCompletionSource is null!", Environment.StackTrace ?? "TDSSmtpClient/GetErrorMessage");
                return language.ERROR_INFO;
            }

            var result = await _sendCompletionSource.Task;
            _sendCompletionSource = null;
            return result;
        }

        public override Task SendAsync(MimeMessage message, CancellationToken cancellationToken = default, ITransferProgress? progress = null)
        {
            _sendCompletionSource = new TaskCompletionSource<string?>();
            return base.SendAsync(message, cancellationToken, progress);
        }

        protected override void OnMessageSent(MessageSentEventArgs e)
        {
            _sendCompletionSource?.TrySetResult(null);
            base.OnMessageSent(e);
        }

        protected override void OnRecipientNotAccepted(MimeMessage message, MailboxAddress mailbox, SmtpResponse response)
        {
            _loggingHandler.LogError($"E-Mail could not be sent to {mailbox.Address}. Status Code: {response.StatusCode}{Environment.NewLine}Response: {response.Response}",
                Environment.StackTrace ?? "OnRecipientNotAccepted");
            _sendCompletionSource?.TrySetResult(_userLanguage.EMAIL_ADDRESS_FOR_ACCOUNT_IS_INVALID);
            base.OnRecipientNotAccepted(message, mailbox, response);
        }

        protected override void OnSenderNotAccepted(MimeMessage message, MailboxAddress mailbox, SmtpResponse response)
        {
            _loggingHandler.LogError($"E-Mail could not be sent. The sender '{mailbox.Address}' has not been accepted. Status Code: {response.StatusCode}{Environment.NewLine}Response: {response.Response}",
                Environment.StackTrace ?? "OnRecipientNotAccepted");
            _sendCompletionSource?.TrySetResult(_userLanguage.ERROR_INFO);
            base.OnSenderNotAccepted(message, mailbox, response);
        }
    }
}
