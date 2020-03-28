using System.Drawing;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Shared.Manager.Utility;

namespace TDS_Server.Handler.Entities.GameModes
{
    partial class Gangwar
    {
        public IMapObject? TargetObject { get; set; }
        public IColShape? TargetColShape { get; set; }

        private IBlip? _targetBlip;
        private ITextLabel? _targetTextLabel;

        private void CreateTargetBlip()
        {
            if (Map.Target is null)
                return;

            _targetBlip = ModAPI.Blip.Create(Map.Target, Lobby.Dimension);
            _targetBlip.Sprite = SharedConstants.TargetBlipSprite;
            _targetBlip.Name = "Target";
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

        private void CreateTargetColShape()
        {
            if (TargetObject is null)
                return;

            TargetColShape = ModAPI.ColShape.CreateSphere(TargetObject.Position, SettingsHandler.ServerSettings.GangwarTargetRadius, Lobby);

            TargetColShape.PlayerEntered += PlayerEnteredTargetColShape;
            TargetColShape.PlayerExited += PlayerExitedTargetColShape;
        }

        private void ClearMapFromTarget()
        {
            _targetBlip?.Delete();
            _targetBlip = null;

            TargetObject?.Delete();
            TargetObject = null;

            _targetTextLabel?.Delete();
            _targetTextLabel = null;
        }
    }
}
