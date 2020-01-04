using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace BonusBotConnector_Server
{
    public class BBCommandService : BBCommand.BBCommandBase
    {
        public delegate void BBUsedCommandDelegate(ulong userId, string command);

        public static event BBUsedCommandDelegate? OnUsedCommand;

        
        public override Task<EmptyReply> UsedCommand(UsedCommandRequest request, ServerCallContext context)
        {
            try
            {
                OnUsedCommand?.Invoke(request.UserId, request.Command);
            }
            catch (Exception ex)
            {
                Program.ErrorLogger(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace);
            }
            return Task.FromResult(new EmptyReply());
        }
    }
}
