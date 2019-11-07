using TDS_Common.Instance.Utility;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class GangActionLobby
    {
        protected EGangActionState _actionState;
        protected TDSTimer? _actionChangeTimer;

        public virtual bool StartPreparations(TDSPlayer attacker)
        {
            if (!CanStartPreparations(attacker))
                return false;
            _actionState = EGangActionState.InPreparation;
            return true;
        }

        public virtual void StopPreparations()
        {
            _actionState = EGangActionState.BeforeAction;
        }

        public virtual bool StartAction()
        {
            _actionState = EGangActionState.InAction;
            return true;
        }

        public virtual void StopAction()
        {
            _actionState = EGangActionState.AfterAction;
        }

        public virtual bool StartEnd()
        {
            _actionState = EGangActionState.InEnd;
            return true;
        }

        public virtual bool StopEnd()
        {
            _actionState = EGangActionState.Completed; 
            return true;
        }
    }
}
