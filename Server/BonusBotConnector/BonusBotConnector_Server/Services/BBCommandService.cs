using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace BonusBotConnector_Server
{
    public class BBCommandService : BBCommand.BBCommandBase
    {
        public delegate string? BBUsedCommandDelegate(ulong userId, string command);

        public static event BBUsedCommandDelegate? OnUsedCommand;

        
        public override Task<UsedCommandReply> UsedCommand(UsedCommandRequest request, ServerCallContext context)
        {
            try
            {
                string? message = OnUsedCommand?.Invoke(request.UserId, request.Command);
                return Task.FromResult(new UsedCommandReply { Message = message ?? "Command sent" });
            }
            catch (Exception ex)
            {
                Program.ErrorLogger(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace);
                return Task.FromResult(new UsedCommandReply { Message = "Error:" + Environment.NewLine + ex.GetBaseException().Message });
            }
            
        }
    }
}
