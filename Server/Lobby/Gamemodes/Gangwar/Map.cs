using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Default;

namespace TDS_Server.LobbySystem.Gamemodes
{
    partial class Gangwar
    {
        private ITDSBlip? _targetBlip;
        private ITDSTextLabel? _targetTextLabel;

        public ITDSColShape? TargetColShape { get; set; }
        public ITDSObject? TargetObject { get; set; }

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

            _targetBlip = NAPI.Blip.CreateBlip(SharedConstants.TargetBlipSprite, Map.Target.ToVector3(), 1f, 0, name: "Target", dimension: Lobby.Dimension) as ITDSBlip;
        }

        private void CreateTargetColShape()
        {
            if (TargetObject is null)
                return;

            TargetColShape = NAPI.ColShape.CreateSphereColShape(TargetObject.Position, (float)SettingsHandler.ServerSettings.GangwarTargetRadius, Lobby.Dimension) as ITDSColShape;

            TargetColShape!.PlayerEntered += PlayerEnteredTargetColShape;
            TargetColShape.PlayerExited += PlayerExitedTargetColShape;
        }

        private void CreateTargetObject()
        {
            if (Map.Target is null)
                return;

            TargetObject = NAPI.Object.CreateObject(NAPI.Util.GetHashKey(SharedConstants.TargetHashName), Map.Target.ToVector3(), new Vector3(), 120, Lobby.Dimension) as ITDSObject;
            TargetObject?.Freeze(true, Lobby);
            TargetObject?.SetCollisionsless(true, Lobby);
        }

        private void CreateTargetTextLabel()
        {
            if (TargetObject is null)
                return;

            _targetTextLabel = NAPI.TextLabel.CreateTextLabel("Target", TargetObject.Position,
                (float)SettingsHandler.ServerSettings.GangwarTargetRadius, 7f, 0, new Color(220, 220, 220), true, Lobby.Dimension) as ITDSTextLabel;
        }
    }
}
