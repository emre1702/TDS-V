using System.Threading.Tasks;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Data.Interfaces.MailSystem
{
    public interface IMailSender
    {
        Task<SmtpClientResponse> SendPasswordResetMail(Players playerEntity, string newPassword, ILanguage language);
    }
}
