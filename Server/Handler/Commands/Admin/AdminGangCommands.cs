using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.CustomAttribute;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.GangSystem;

namespace TDS.Server.Handler.Commands.Admin
{
    public class AdminGangCommands
    {
        private readonly GangLevelsHandler _gangLevelsHandler;
        private readonly GangHousesHandler _gangHousesHandler;

        public AdminGangCommands(GangLevelsHandler gangLevelsHandler, GangHousesHandler gangHousesHandler) 
            => (_gangLevelsHandler, _gangHousesHandler) = (gangLevelsHandler, gangHousesHandler);

        [TDSCommandAttribute(AdminCommand.CreateHouse)]
        public async Task CreateHouse(ITDSPlayer player, byte neededGangLevel)
        {
            if (player is null || player.Entity is null)
                return;

            if (!(player.Lobby is IGangLobby))
            {
                NAPI.Task.RunSafe(() => player.SendNotification(player.Language.ONLY_ALLOWED_IN_GANG_LOBBY));
                return;
            }

            if (neededGangLevel > _gangLevelsHandler.HighestLevel)
            {
                NAPI.Task.RunSafe(() => player.SendNotification(string.Format(player.Language.GANG_LEVEL_MAX_ALLOWED, _gangLevelsHandler.HighestLevel)));
                return;
            }

            await _gangHousesHandler.AddHouse(player.Position, player.Rotation.Z, neededGangLevel, player.Entity.Id).ConfigureAwait(false);
            NAPI.Task.RunSafe(() => player.SendNotification(player.Language.ADDED_THE_GANG_HOUSE_SUCCESSFULLY));
        }
    }
}
