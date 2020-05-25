using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Control;

namespace TDS_Client.RAGEAPI.Control
{
    internal class ControlAPI : IControlAPI
    {
        #region Public Methods

        public void DisableAllControlActions(InputGroup inputGroup)
        {
            RAGE.Game.Pad.DisableAllControlActions((int)inputGroup);
        }

        public void DisableControlAction(InputGroup inputGroup, Data.Enums.Control control, bool disable = true)
        {
            RAGE.Game.Pad.DisableControlAction((int)inputGroup, (int)control, disable);
        }

        public float GetDisabledControlNormal(InputGroup inputGroup, Data.Enums.Control control)
        {
            return RAGE.Game.Pad.GetDisabledControlNormal((int)inputGroup, (int)control);
        }

        public bool IsControlJustPressed(InputGroup inputGroup, Data.Enums.Control control)
        {
            return RAGE.Game.Pad.IsControlJustPressed((int)inputGroup, (int)control);
        }

        public bool IsControlJustReleased(InputGroup inputGroup, Data.Enums.Control control)
        {
            return RAGE.Game.Pad.IsControlJustReleased((int)inputGroup, (int)control);
        }

        public bool IsControlPressed(InputGroup inputGroup, Data.Enums.Control control)
        {
            return RAGE.Game.Pad.IsControlPressed((int)inputGroup, (int)control);
        }

        public bool IsDisabledControlJustPressed(InputGroup inputGroup, Data.Enums.Control control)
        {
            return RAGE.Game.Pad.IsDisabledControlJustPressed((int)inputGroup, (int)control);
        }

        public bool IsDisabledControlJustReleased(InputGroup inputGroup, Data.Enums.Control control)
        {
            return RAGE.Game.Pad.IsDisabledControlJustReleased((int)inputGroup, (int)control);
        }

        public bool IsDisabledControlPressed(InputGroup inputGroup, Data.Enums.Control control)
        {
            return RAGE.Game.Pad.IsDisabledControlPressed((int)inputGroup, (int)control);
        }

        public bool SetControlNormal(InputGroup inputGroup, Data.Enums.Control control, float amount)
        {
            return RAGE.Game.Pad.SetControlNormal((int)inputGroup, (int)control, amount);
        }

        #endregion Public Methods
    }
}
