using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.PlayersSystem;
using TDS_Server.Handler.GangSystem;

namespace TDS_Server.PlayersSystem
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
