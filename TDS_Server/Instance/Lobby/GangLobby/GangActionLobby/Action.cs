using TDS_Common.Instance.Utility;
using TDS_Server.Enum;

namespace TDS_Server.Instance.Lobby
{
    partial class GangActionLobby
    {
        protected EGangActionState _actionState;
        protected TDSTimer? _actionChangeTimer;

        public virtual void StartPreparations()
        {
            _actionState = EGangActionState.InPreparation;
        }

        public virtual void StopPreparations()
        {
            _actionState = EGangActionState.BeforeAction;
        }

        public virtual void StartAction()
        {
            _actionState = EGangActionState.InAction;
        }

        public virtual void StopAction()
        {
            _actionState = EGangActionState.AfterAction;
        }

        public virtual void StartEnd()
        {
            _actionState = EGangActionState.InEnd;
        }

        public virtual void StopEnd()
        {
            _actionState = EGangActionState.Completed;
        }
    }
}
