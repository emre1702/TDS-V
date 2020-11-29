using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Models;

namespace TDS.Server.Data.Interfaces
{
#nullable enable
    public interface IInvitation
    {
        InvitationDto Dto { get; set; }
        bool RemoveOnLobbyLeave { get; set; }
        ITDSPlayer? Sender { get; set; }
        ITDSPlayer Target { get; set; }
        InvitationType Type { get; set; }
        bool Withdrawn { get; }

        void Accept();
        void Reject();
        void Show();
        void Withdraw();
    }
}