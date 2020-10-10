using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.LobbySystem.Weapons
{
    public interface IFightLobbyWeapons
    {
        void GivePlayerWeapons(ITDSPlayer player);
    }
}
