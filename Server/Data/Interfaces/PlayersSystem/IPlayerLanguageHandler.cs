using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Data.Interfaces.PlayersSystem
{
    public interface IPlayerLanguageHandler
    {
        ILanguage Data { get; set; }
        Language EnumValue { get; set; }

        void Init(ITDSPlayer player, IPlayerEvents events);
    }
}
