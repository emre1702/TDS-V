using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.LobbySystem.Natives
{
    public interface IBaseLobbyNatives
    {
        void Send(NativeHash nativeHash, params object[] args);
    }
}
