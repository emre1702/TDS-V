using BonusBotConnector.Server;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Userpanel;
using TDS_Shared.Data.Enums.Userpanel;

namespace BonusBotConnector_Server
{
    public class SupportRequestService : SupportRequest.SupportRequestBase
    {
        private readonly ILoggingHandler _loggingHandler;
        private readonly IUserpanelHandler _userpanelHandler;

        public SupportRequestService(ILoggingHandler loggingHandler, IUserpanelHandler userpanelHandler)
            => (_loggingHandler, _userpanelHandler) = (loggingHandler, userpanelHandler);

        public override async Task<Reply> Create(CreateRequest request, ServerCallContext context)
        {
            try
            {
                var msg = string.Empty;
                var task = _userpanelHandler.SupportRequestHandler?.CreateRequestFromDiscord(request.UserId, request.Title, request.Text,
                    (SupportType)request.Type, request.AtleastAdminLevel);
                if (task is { })
                    msg = await task ?? string.Empty;
                return new Reply { Message = msg };
            }
            catch (Exception ex)
            {
                _loggingHandler.LogErrorFromBonusBot(ex, false);
                var baseEx = ex.GetBaseException();
                return new Reply { Message = $"[{ex.GetType().Name}|{baseEx.GetType().Name}] + Error:" + Environment.NewLine + baseEx.Message };
            }
        }

        public override async Task<Reply> Answer(AnswerRequest request, ServerCallContext context)
        {
            try
            {
                var msg = string.Empty;
                var task = _userpanelHandler.SupportRequestHandler?.AnswerRequestFromDiscord(request.UserId, request.SupportRequestId, request.Text);
                if (task is { })
                    msg = await task ?? string.Empty;
                return new Reply { Message = msg };
            }
            catch (Exception ex)
            {
                _loggingHandler.LogErrorFromBonusBot(ex, false);
                var baseEx = ex.GetBaseException();
                return new Reply { Message = $"[{ex.GetType().Name}|{baseEx.GetType().Name}] + Error:" + Environment.NewLine + baseEx.Message };
            }
        }

        public override async Task<Reply> ToggleClosed(ToggleClosedRequest request, ServerCallContext context)
        {
            try
            {
                var msg = string.Empty;
                var task = _userpanelHandler.SupportRequestHandler?.ToggleClosedRequestFromDiscord(request.UserId, request.SupportRequestId, request.Closed);
                if (task is { })
                    msg = await task ?? string.Empty;
                return new Reply { Message = msg };
            }
            catch (Exception ex)
            {
                _loggingHandler.LogErrorFromBonusBot(ex, false);
                var baseEx = ex.GetBaseException();
                return new Reply { Message = $"[{ex.GetType().Name}|{baseEx.GetType().Name}] + Error:" + Environment.NewLine + baseEx.Message };
            }
        }
    }
}
