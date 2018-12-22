using TDS.server.enums;

namespace TDS.server.instance.lobby.interfaces {

    interface IRound {
        void StartRoundGame ( );
        void StartMapChoose ( );
        void EndRoundEarlier ( RoundEndReason reason, object arg = null );
        void CheckForEnoughAlive ( );
    }
}
