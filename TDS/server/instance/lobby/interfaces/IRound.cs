using GTANetworkAPI;

namespace TDS.server.instance.lobby.interfaces {

    interface IRound {
        void StartRoundGame ( );
        void StartMapChoose ( );
        void EndRoundEarlier ( );
        void CheckForEnoughAlive ( );
    }
}
