using TDS_Server.Data.Enums;

namespace TDS_Server.MailSystem.Datas
{
    internal class SubjectProvider
    {
        public string Get(MailType type)
            => type switch
            {
                MailType.PasswordReset => "Password reset has been requested.",

                _ => "Mail from TDS-V"
            };
    }
}
