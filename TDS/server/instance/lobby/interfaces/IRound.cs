using GTANetworkAPI;
using System.Threading.Tasks;

namespace TDS.server.instance.lobby.interfaces {

    interface IRound {
        void StartRoundGame ( );
        void StartMapChoose ( );
        void EndRoundEarlier ( );
        void CheckForEnoughAlive ( );
    }
}
