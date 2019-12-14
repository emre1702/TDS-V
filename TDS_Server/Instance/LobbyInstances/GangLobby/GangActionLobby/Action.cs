using TDS_Common.Instance.Utility;
using TDS_Server.Enums;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class GangActionLobby
    {
        protected EGangActionState _actionState;
        protected TDSTimer? _actionChangeTimer;

        protected bool InPreparation => _actionState == EGangActionState.InPreparation;
        protected bool InAction => _actionState == EGangActionState.BeforeAction || _actionState == EGangActionState.InAction;

        public virtual bool StartPreparations()
        {
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

            LobbyEntity.Name = $"[{ActionTypeShort}] {AttackerGang.Entity.Short} - {OwnerGang.Entity.Short}";

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

            AttackerGang.InAction = false;
            OwnerGang.InAction = false;

            Remove();

            return true;
        }
    }
}
