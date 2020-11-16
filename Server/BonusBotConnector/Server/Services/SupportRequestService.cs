using BonusBotConnector.Server;
using Grpc.Core;
using System;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.Userpanel;
using TDS.Shared.Data.Enums.Userpanel;

namespace BonusBotConnector_Server
{
    public class SupportRequestService : SupportRequest.SupportRequestBase
    {
        #region Private Fields

        private readonly ILoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public SupportRequestService(ILoggingHandler loggingHandler)
            => (_loggingHandler) = (loggingHandler);

        #endregion Public Constructors

        #region Public Delegates

        public delegate Task<string?> AnswerRequestFromDiscordDelegate(ulong discordUserId, int requestId, string text);

        public delegate Task<string?> CreateRequestFromDiscordDelegate(ulong discordUserId, string title, string text, SupportType supportType, int atleastAdminLevel);

        public delegate Task<string?> ToggleClosedRequestFromDiscordDelegate(ulong discordUserId, int requestId, bool closed);

        #endregion Public Delegates

        #region Public Events

        public event AnswerRequestFromDiscordDelegate? AnswerRequestFromDiscord;

        public event CreateRequestFromDiscordDelegate? CreateRequestFromDiscord;

        public event ToggleClosedRequestFromDiscordDelegate? ToggleClosedRequestFromDiscord;

        #endregion Public Events

        #region Public Methods

        public override async Task<Reply> Answer(AnswerRequest request, ServerCallContext context)
        {
            try
            {
                var msg = string.Empty;
                var task = AnswerRequestFromDiscord?.Invoke(request.UserId, request.SupportRequestId, request.Text);
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

        public override async Task<Reply> Create(CreateRequest request, ServerCallContext context)
        {
            try
            {
                var msg = string.Empty;
                var task = CreateRequestFromDiscord?.Invoke(request.UserId, request.Title, request.Text,
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

        public override async Task<Reply> ToggleClosed(ToggleClosedRequest request, ServerCallContext context)
        {
            try
            {
                var msg = string.Empty;
                var task = ToggleClosedRequestFromDiscord?.Invoke(request.UserId, request.SupportRequestId, request.Closed);
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

        #endregion Public Methods
    }
}
