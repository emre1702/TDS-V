using TDS_Client.Handler.Entities.GTA;

namespace TDS_Client.Handler.Factories
{
    public class TextLabelFactory
    {
        public TextLabelFactory()
        {
            RAGE.Elements.Entities.TextLabels.CreateEntity =
                (ushort id, ushort remoteId) => new TDSTextLabel(id, remoteId);
        }
    }
}
