using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Interfaces.LobbySystem.Weapons
{
    public interface IFightLobbyWeapons
    {
        void GivePlayerWeapons(ITDSPlayer player);
    }
}
