using System;
using System.Drawing;
using TDS_Client.Data.Interfaces.ModAPI.Checkpoint;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Checkpoint
{
    internal class Checkpoint : RAGE.Elements.Checkpoint, ICheckpoint
    {
        #region Public Constructors

        public Checkpoint(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        public Checkpoint(uint hash, Position3D position, float radius, Position3D direction, Color color, bool isVisible = true, uint dimension = 0)
            : base(hash, position.ToVector3(), radius, direction.ToVector3(), color.ToRGBA(), isVisible, dimension)
        {
            base.OnEnter += this.OnEnterMethod;
            base.OnExit += this.OnExitMethod;
        }

        #endregion Public Constructors

        #region Public Events

        public new event Data.Interfaces.ModAPI.Checkpoint.CheckpointEventDelegate OnEnter;

        public new event Data.Interfaces.ModAPI.Checkpoint.CheckpointEventDelegate OnExit;

        #endregion Public Events

        #region Public Properties

        public new Position3D Direction
        {
            get => base.Direction.ToPosition3D();
            set => base.Direction = value.ToVector3();
        }

        #endregion Public Properties

        #region Private Methods

        private void OnEnterMethod(RAGE.Events.CancelEventArgs modCancelEventArgs)
        {
            var cancelEventArgs = new CancelEventArgs(modCancelEventArgs.Cancel);
            OnEnter?.Invoke(cancelEventArgs);
            modCancelEventArgs.Cancel = cancelEventArgs.Cancel;
        }

        private void OnExitMethod(RAGE.Events.CancelEventArgs modCancelEventArgs)
        {
            var cancelEventArgs = new CancelEventArgs(modCancelEventArgs.Cancel);
            OnExit?.Invoke(cancelEventArgs);
            modCancelEventArgs.Cancel = cancelEventArgs.Cancel;
        }

        #endregion Private Methods
    }
}
