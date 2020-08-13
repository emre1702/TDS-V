using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Checkpoint
{
    public delegate void CheckpointEventDelegate(CancelEventArgs cancel);

    public interface ICheckpoint
    {
        #region Public Events

        event CheckpointEventDelegate OnEnter;

        event CheckpointEventDelegate OnExit;

        #endregion Public Events

        #region Public Properties

        Position Direction { get; set; }
        float Radius { get; set; }
        bool Visible { get; set; }

        #endregion Public Properties
    }
}
