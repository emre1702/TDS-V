using GTANetworkAPI;
using TDS_Common.Manager.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar
    {
        public Object? TargetObject { get; set; }

        private Blip? _targetBlip;
        private TextLabel? _targetTextLabel;

        private void CreateTargetBlip()
        {
            if (Map.Target is null)
                return;

            _targetBlip = NAPI.Blip.CreateBlip(Map.Target.ToVector3(), Lobby.Dimension);
            _targetBlip.Sprite = Constants.TargetBlipSprite;
            _targetBlip.Name = "Target";
        }

        private void CreateTargetObject()
        {
            if (Map.Target is null)
                return;

            TargetObject = NAPI.Object.CreateObject(ServerConstants.TargetHash, Map.Target.ToVector3(),new Vector3(), 120, Lobby.Dimension);
            Workaround.FreezeEntity(TargetObject, true, Lobby);
            Workaround.SetEntityCollisionless(TargetObject, true, Lobby);
        }

        private void CreateTargetTextLabel()
        {
            if (TargetObject is null)
                return;

            _targetTextLabel = NAPI.TextLabel.CreateTextLabel("Target", TargetObject.Position, 
                (float)SettingsManager.ServerSettings.GangwarTargetRadius, 7f, 0, new Color(220, 220, 220), true, Lobby.Dimension);
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
