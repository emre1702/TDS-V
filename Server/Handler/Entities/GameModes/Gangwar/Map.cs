using System.Drawing;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Shared.Data.Default;

namespace TDS_Server.Handler.Entities.GameModes
{
    partial class Gangwar
    {
        #region Private Fields

        private IBlip? _targetBlip;
        private ITextLabel? _targetTextLabel;

        #endregion Private Fields

        #region Public Properties

        public IColShape? TargetColShape { get; set; }
        public IMapObject? TargetObject { get; set; }

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

            _targetBlip = ModAPI.Blip.Create(SharedConstants.TargetBlipSprite, Map.Target, name: "Target", dimension: Lobby.Dimension);
        }

        private void CreateTargetColShape()
        {
            if (TargetObject is null)
                return;

            TargetColShape = ModAPI.ColShape.CreateSphere(TargetObject.Position, SettingsHandler.ServerSettings.GangwarTargetRadius, Lobby);

            TargetColShape.PlayerEntered += PlayerEnteredTargetColShape;
            TargetColShape.PlayerExited += PlayerExitedTargetColShape;
        }

        private void CreateTargetObject()
        {
            if (Map.Target is null)
                return;

            TargetObject = ModAPI.MapObject.Create(SharedConstants.TargetHashName, Map.Target, null, 120, Lobby);
            TargetObject.Freeze(true, Lobby);
            TargetObject.SetCollisionsless(true, Lobby);
        }

        private void CreateTargetTextLabel()
        {
            if (TargetObject is null)
                return;

            _targetTextLabel = ModAPI.TextLabel.Create("Target", TargetObject.Position,
                SettingsHandler.ServerSettings.GangwarTargetRadius, 7f, 0, Color.FromArgb(220, 220, 220), true, Lobby);
        }

        #endregion Private Methods
    }
}
