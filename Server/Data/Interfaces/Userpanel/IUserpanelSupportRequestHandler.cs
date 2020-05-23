using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Shared.Data.Enums.Userpanel;

namespace TDS_Server.Data.Interfaces.Userpanel
{
    #nullable enable
    public interface IUserpanelSupportRequestHandler
    {
        Task<string?> GetSupportRequests(ITDSPlayer player);
        Task<string?> CreateRequestFromDiscord(ulong discordUserId, string title, string text, SupportType supportType, int atleastAdminLevel);
        Task<string?> AnswerRequestFromDiscord(ulong discordUserId, int requestId, string text);
        Task<string?> ToggleClosedRequestFromDiscord(ulong discordUserId, int requestId, bool closed);
        Task<object?> GetSupportRequestData(ITDSPlayer player, ArraySegment<object> args);
        Task<object?> SetSupportRequestClosed(ITDSPlayer player, ArraySegment<object> args);
        Task<object?> SendRequest(ITDSPlayer player, ArraySegment<object> args);
        Task<object?> SendMessage(ITDSPlayer player, ArraySegment<object> args);
        object? LeftSupportRequest(ITDSPlayer player, ref ArraySegment<object> args);
        object? LeftSupportRequestsList(ITDSPlayer player, ref ArraySegment<object> args);
    }
}
