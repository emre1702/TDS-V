using System.Drawing;
using TDS_Client.Data.Interfaces.ModAPI.Checkpoint;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Checkpoint
{
    internal class CheckpointAPI : ICheckpointAPI
    {
        #region Public Methods

        public ICheckpoint Create(uint hash, Position3D position, float radius, Position3D direction, Color color, bool isVisible = true, uint dimension = 0)
        {
            return new Checkpoint(hash, position, radius, direction, color, isVisible, dimension);
        }

        #endregion Public Methods
    }
}
