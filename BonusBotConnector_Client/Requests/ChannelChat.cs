using Grpc.Net.Client;
using System;
using static BonusBotConnector_Client.Main;
using static BonusBotConnector_Client.MessageToChannel;

namespace BonusBotConnector_Client.Requests
{
    public static class ChannelChat
    {
        private static MessageToChannelClient? _client;
        private static BonusBotErrorLoggerDelegate? _errorLogger;

        internal static void Init(GrpcChannel channel, BonusBotErrorLoggerDelegate errorLogger)
        {
            _client = new MessageToChannelClient(channel);
            _errorLogger = errorLogger;
        }

        public static void SendAdminApplication(string info)
        {
            SendRequest(info, Settings?.AdminApplicationsChannelId);
        }

        public static void SendSupportRequest(string info)
        {
            SendRequest(info, Settings?.SupportRequestsChannelId);
        }

        public static void SendError(string msg)
        {
            SendRequest(msg, Settings?.ErrorLogsChannelId, false);
        }

        public static void SendActionInfo(string info)
        {
            SendRequest(info, Settings?.ActionsInfoChannelId);
        }

        private static async void SendRequest(string text, ulong? channelId, bool logToBonusBotOnError = true) 
        {
            if (_client is null)
                return;
            if (Settings is null)
                return;
            if (Settings.GuildId is null)
                return;
            if (channelId is null)
                return;
            try
            {
                var result = await _client.SendAsync(new MessageToChannelRequest { GuildId = Settings.GuildId.Value, ChannelId = channelId.Value, Text = text });
                HandleResult(result);
            }
            catch (Exception ex)
            {
                _errorLogger?.Invoke(ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, logToBonusBotOnError);
            }
        }

        private static void HandleResult(MessageToChannelRequestReply result)
        {
            if (string.IsNullOrEmpty(result.ErrorMessage))
                return;
            _errorLogger?.Invoke(result.ErrorMessage, Environment.StackTrace, true);
        }
    }
}
