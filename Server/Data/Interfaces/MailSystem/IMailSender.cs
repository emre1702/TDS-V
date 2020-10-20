using System.Threading.Tasks;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Data.Interfaces.MailSystem
{
    public interface IMailSender
    {
        Task<SmtpClientResponse> SendPasswordResetMail(Players playerEntity, string newPassword, ILanguage language);
    }
}
