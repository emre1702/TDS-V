using System;
using System.Threading.Tasks;
using BonusBotConnector.Server;
using Grpc.Core;
using TDS_Server.Data.Interfaces;

namespace BonusBotConnector_Server
{
    public class BBCommandService : BBCommand.BBCommandBase
    {
        public delegate string? BBUsedCommandDelegate(ulong userId, string command);

        public event BBUsedCommandDelegate? OnUsedCommand;

        private readonly ILoggingHandler _loggingHandler;

        public BBCommandService(ILoggingHandler loggingHandler)
            => _loggingHandler = loggingHandler;

        public override Task<UsedCommandReply> UsedCommand(UsedCommandRequest request, ServerCallContext context)
        {
            try
            {
                string? message = OnUsedCommand?.Invoke(request.UserId, request.Command);
                return Task.FromResult(new UsedCommandReply { Message = message ?? "Command was sent" });
            }
            catch (Exception ex)
            {
                _loggingHandler.LogErrorFromBonusBot(ex, false);
                return Task.FromResult(new UsedCommandReply { Message = "Error:" + Environment.NewLine + ex.GetBaseException().Message });
            }
            
        }
    }
}
