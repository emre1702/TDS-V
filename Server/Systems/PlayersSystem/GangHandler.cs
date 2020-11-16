using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.PlayersSystem;
using TDS.Server.Handler.GangSystem;

namespace TDS.Server.PlayersSystem
{
    public class GangHandler : IPlayerGangHandler
    {
        private IGang? _gang;

        public IGang Gang
        {
            get
            {
                if (_gang is null)
                {
                    _gang = _gangsHandler.None;
                }
                return _gang;
            }
            set => _gang = value;
        }

        private readonly GangsHandler _gangsHandler;

        public GangHandler(GangsHandler gangsHandler)
        {
            _gangsHandler = gangsHandler;
        }
    }
}
