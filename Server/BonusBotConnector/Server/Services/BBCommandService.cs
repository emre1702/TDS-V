using BonusBotConnector.Server;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Utility;

namespace BonusBotConnector_Server
{
    public class BBCommandService : BBCommand.BBCommandBase
    {
        #region Public Fields

        public AsyncValueTaskEvent<(ulong userId, string command, IList<string> args, BBUsedCommandReply reply)>? OnUsedCommand;

        #endregion Public Fields

        #region Private Fields

        private readonly ILoggingHandler _loggingHandler;

        #endregion Private Fields

        #region Public Constructors

        public BBCommandService(ILoggingHandler loggingHandler)
            => _loggingHandler = loggingHandler;

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods
    }

    public class BBUsedCommandReply
    {
        #region Public Properties

        public string? Message { get; set; }

        #endregion Public Properties
    }
}
