using System.Linq;
using TDS_Server.Core.Manager.Utility;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Models.Map;
using TDS_Server.Database.Entity;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Entities.GameModes.Bomb
{
    partial class Bomb : GameMode
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

        #region Public Methods

        public static void Init(TDSDbContext dbContext)
        {
            _allowedWeaponHashes = dbContext.Weapons
                .Select(w => w.Hash)
                .ToHashSet();
        }

        #endregion Public Methods
    }
}
