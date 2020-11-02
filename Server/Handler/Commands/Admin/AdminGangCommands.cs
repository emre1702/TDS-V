using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.CustomAttribute;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.GangSystem;

namespace TDS_Server.Handler.Commands.Admin
{
    public class AdminGangCommands
    {
        private readonly GangLevelsHandler _gangLevelsHandler;
        private readonly GangHousesHandler _gangHousesHandler;

        public AdminGangCommands(GangLevelsHandler gangLevelsHandler, GangHousesHandler gangHousesHandler) 
            => (_gangLevelsHandler, _gangHousesHandler) = (gangLevelsHandler, gangHousesHandler);

        [TDSCommand(AdminCommand.CreateHouse)]
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
