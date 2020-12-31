using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums.CharCreator;

namespace TDS.Client.Handler.Appearance.CharCreator.Body
{
    internal class BodyNavCameraHandler
    {
        private readonly CharCreatorCameraHandler _cameraHandler;

        public BodyNavCameraHandler(CharCreatorCameraHandler cameraHandler)
        {
            _cameraHandler = cameraHandler;

            RAGE.Events.Add(FromBrowserEvent.BodyNavChanged, NavChanged);
        }

        private void NavChanged(object[] args)
        {
            _cameraHandler.SetCameraTarget(CharCreatorCameraTarget.Head);
        }
    }
}