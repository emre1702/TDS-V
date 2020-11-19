using TDS.Client.Handler.Events;
using TDS.Shared.Default;
using static RAGE.Events;

namespace TDS.Client.Handler.Lobby.Bomb
{
    internal class BombOnHandHandler
    {
        public bool BombOnHand { get; private set; }

        internal BombOnHandHandler(EventsHandler eventsHandler)
        {
            eventsHandler.LocalPlayerDied += SetBombNotOnHand;

            Add(ToClientEvent.BombOnHand, _ => SetBombOnHand());
            Add(ToClientEvent.BombNotOnHand, _ => SetBombNotOnHand());
        }

        internal void SetBombOnHand()
        {
            BombOnHand = true;
        }

        internal void SetBombNotOnHand()
        {
            BombOnHand = false;
        }
    }
}
