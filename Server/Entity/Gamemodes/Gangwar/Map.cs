using AltV.Net;
using AltV.Net.Data;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Models;
using TDS_Shared.Data.Default;

namespace TDS_Server.Entity.Gamemodes.Gangwar
{
    partial class Gangwar
    {
        #region Private Fields

        private ITDSBlip? _targetBlip;
        private ITDSTextLabel? _targetTextLabel;

        #endregion Private Fields

        #region Public Properties

        public ITDSColShape? TargetColShape { get; set; }
        public ITDSObject? TargetObject { get; private set; }

        #endregion Public Properties

        #region Private Methods

        private void ClearMapFromTarget()
        {
            _targetBlip?.Delete();
            _targetBlip = null;

            TargetObject?.Delete();
            TargetObject = null;

            _targetTextLabel?.Delete();
            _targetTextLabel = null;
        }

        private void CreateTargetBlip()
        {
            if (Map.Target is null)
                return;

            _targetBlip = _tdsBlipHandler.Create(SharedConstants.TargetBlipSprite, Map.Target.ToAltV(), name: "Target", dimension: (int)Lobby.Dimension);
        }

        private void CreateTargetColShape()
        {
            if (TargetObject is null)
                return;

            TargetColShape = (ITDSColShape)Alt.CreateColShapeSphere(TargetObject.Position, (float)SettingsHandler.ServerSettings.GangwarTargetRadius);
            TargetColShape.Dimension = (int)Lobby.Dimension;

            TargetColShape.PlayerEntered += PlayerEnteredTargetColShape;
            TargetColShape.PlayerExited += PlayerExitedTargetColShape;
        }

        private void CreateTargetObject()
        {
            if (Map.Target is null)
                return;

            TargetObject =_tdsObjectHandler.Create(SharedConstants.TargetHashName, Map.Target.ToAltV(), new DegreeRotation(), 120, (int)Lobby.Dimension);
            TargetObject.Freeze(true, Lobby);
            TargetObject.SetCollisionsless(true, Lobby);
        }

        private void CreateTargetTextLabel()
        {
            if (TargetObject is null)
                return;

            _targetTextLabel = _tdsTextLabelHandler.Create("Target", TargetObject.Position,
                SettingsHandler.ServerSettings.GangwarTargetRadius, 7f, 0, new Color(220, 220, 220), true, (int)Lobby.Dimension);
        }

        #endregion Private Methods
    }
}
