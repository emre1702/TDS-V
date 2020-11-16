using BonusBotConnector.Server;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Utility;

namespace BonusBotConnector_Server
{
    public class BBCommandService : BBCommand.BBCommandBase
    {

        public AsyncValueTaskEvent<(ulong userId, string command, IList<string> args, BBUsedCommandReply reply)>? OnUsedCommand { get; set; }

        private readonly ILoggingHandler _loggingHandler;

        public BBCommandService(ILoggingHandler loggingHandler)
            => _loggingHandler = loggingHandler;

        public override async Task<UsedCommandReply> UsedCommand(UsedCommandRequest request, ServerCallContext context)
        {
            try
            {
                var reply = new BBUsedCommandReply();
                var task = OnUsedCommand?.InvokeAsync((request.UserId, request.Command, request.Args, reply));
                if (task is { })
                    await task.Value;
                return new UsedCommandReply { Message = reply.Message ?? string.Empty };
            }
            catch (Exception ex)
            {
                _loggingHandler.LogErrorFromBonusBot(ex, false);
                return new UsedCommandReply { Message = ex.GetType().Name + " error:" + Environment.NewLine + ex.GetBaseException().Message };
            }
        }

    }

    public class BBUsedCommandReply
    {

        public string? Message { get; set; }

    }
}
