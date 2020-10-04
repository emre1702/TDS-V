using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.Players
{
#nullable enable

    public interface IBombGamemodePlayers
    {
        ITDSPlayer? BombAtPlayer { get; set; }
        ITDSPlayer? Planter { get; set; }
    }
}
