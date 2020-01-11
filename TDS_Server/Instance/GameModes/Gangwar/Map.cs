using GTANetworkAPI;
using TDS_Common.Manager.Utility;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.GameModes
{
    partial class Gangwar
    {
        private Blip? _targetBlip;
        private Object? _targetObject;

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

            _targetObject = NAPI.Object.CreateObject(ServerConstants.TargetHash, Map.Target.ToVector3(),new Vector3(), 120, Lobby.Dimension);
            Workaround.SetEntityCollisionless(_targetObject, true, Lobby);
        }

        private void ClearMapFromTarget()
        {
            _targetBlip?.Delete();
            _targetBlip = null;

            _targetObject?.Delete();
            _targetObject = null;
        }
    }
}
