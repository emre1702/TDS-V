using System.Drawing;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Checkpoint
{
    public interface ICheckpointAPI
    {
        #region Public Methods

        ICheckpoint Create(uint hash, Position3D position, float radius, Position3D direction, Color color, bool isVisible = true, uint dimension = 0);

        #endregion Public Methods
    }
}
