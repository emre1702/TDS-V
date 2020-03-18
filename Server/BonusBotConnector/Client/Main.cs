namespace BonusBotConnector.Client
{
    public class BonusBotConnectorClient
    {
        public BonusbotSettings? Settings { get; private set; }
        // public static GrpcChannel? Channel { get; private set; }

        public delegate void BonusBotErrorLoggerDelegate(string info, string stackTrace, bool logToBonusBot = true);

        public BonusBotConnectorClient(TDSDbContext dbContext, BonusBotErrorLoggerDelegate errorLogger)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                return;

            Settings = dbContext.BonusbotSettings.FirstOrDefault();

            if (Settings.GuildId is null)
                return;

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            var channel = GrpcChannel.ForAddress("http://localhost:5000");

            ChannelChat.Init(channel, errorLogger);
            ServerInfos.Init(channel, errorLogger);
            PrivateChat.Init(channel, errorLogger);
        }
    }
}
