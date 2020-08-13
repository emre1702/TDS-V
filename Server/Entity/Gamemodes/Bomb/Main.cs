using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities.Gamemodes;
using TDS_Server.Data.Models.Map;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Entity.Gamemodes.Bomb
{
    partial class Bomb : Gamemode, IBomb
    {
        #region Private Fields

        private readonly ITeam _counterTerroristTeam;
        private readonly ITeam _terroristTeam;
        private ITDSPlayer? _bombAtPlayer;

        #endregion Private Fields

        #region Public Constructors

        public Bomb(Arena arena, MapDto map, IModAPI modAPI, Serializer serializer, ISettingsHandler settingsHandler, LangHelper langHelper, InvitationsHandler invitationsHandler)
            : base(arena, map, modAPI, serializer, settingsHandler, langHelper, invitationsHandler)
        {
            _terroristTeam = arena.Teams[2];
            _counterTerroristTeam = arena.Teams[1];
        }

        #endregion Public Constructors
    }
}
