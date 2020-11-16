using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Interfaces.LobbySystem.Natives
{
    public interface IBaseLobbyNatives
    {
        void Send(NativeHash nativeHash, params object[] args);
    }
}
