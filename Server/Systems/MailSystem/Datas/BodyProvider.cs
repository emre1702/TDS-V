using TDS_Server.Data.Enums;

namespace TDS_Server.MailSystem.Datas
{
    internal class BodyProvider
    {
        public string Get(MailType type, params object?[] args)
            => string.Format(type switch
            {
                MailType.PasswordReset => GetPasswordResetBody(),

                _ => ""
            }, args);

        private string GetPasswordResetBody()
            => @"You have (or someone else has) requested a password reset for your account.
The system has generated a random password only for you.
Use it to login to your account and change the password in the special settings window in the Userpanel.

New password:
{0}";
    }
}
