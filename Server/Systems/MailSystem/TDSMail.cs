using MimeKit;
using TDS_Server.Data.Models;

namespace TDS_Server.MailSystem
{
    internal class TDSMail : MimeMessage
    {
        public TDSMail(TDSMailOptions options)
        {
            From.Add(new MailboxAddress(options.SenderName, options.SenderAddress));
            To.Add(MailboxAddress.Parse(options.ReceiverAddress));
            Subject = options.Subject;
            Body = new TextPart("plain") { Text = options.Body };
        }
    }
}
