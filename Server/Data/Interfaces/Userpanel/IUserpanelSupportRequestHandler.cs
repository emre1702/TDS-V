using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Shared.Data.Enums.Userpanel;

namespace TDS.Server.Data.Interfaces.Userpanel
{
#nullable enable

    public interface IUserpanelSupportRequestHandler
    {
        #region Public Methods

        Task<string?> AnswerRequestFromDiscord(ulong discordUserId, int requestId, string text);

        Task<string?> CreateRequestFromDiscord(ulong discordUserId, string title, string text, SupportType supportType, int atleastAdminLevel);

        Task<string?> GetSupportRequests(ITDSPlayer player);

        Task<string?> ToggleClosedRequestFromDiscord(ulong discordUserId, int requestId, bool closed);

        #endregion Public Methods
    }
}