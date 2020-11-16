using TDS.Client.Handler.Entities.GTA;

namespace TDS.Client.Handler.Factories
{
    public class CheckpointFactory
    {
        public CheckpointFactory()
            => RAGE.Elements.Entities.Checkpoints.CreateEntity =
                (ushort id, ushort remoteId) => new TDSCheckpoint(id, remoteId);
    }
}
