using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerLanguageHandler
    {
        ILanguage Data { get; set; }
        Language Enum { get; set; }

        void Init(ITDSPlayer player, IPlayerEvents events);
    }
}
