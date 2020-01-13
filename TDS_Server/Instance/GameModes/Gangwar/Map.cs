using GTANetworkAPI;
using TDS_Common.Manager.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar
    {
        public Object? TargetObject { get; set; }

        private Blip? _targetBlip;

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
            Workaround.SetEntityCollisionless(TargetObject, true, Lobby);
        }

        private void ClearMapFromTarget()
        {
            _targetBlip?.Delete();
            _targetBlip = null;

            TargetObject?.Delete();
            TargetObject = null;
        }
    }
}
