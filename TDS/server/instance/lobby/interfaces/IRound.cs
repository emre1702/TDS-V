using GTANetworkAPI;
using System.Threading.Tasks;

namespace TDS.server.instance.lobby.interfaces {

    interface IRound {
        Task StartRoundGame ( );
        Task StartMapChoose ( );
        void EndRoundEarlier ( );
        void CheckForEnoughAlive ( );
    }
}
