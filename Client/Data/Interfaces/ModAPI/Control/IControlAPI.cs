using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Control
{
    public interface IControlAPI
    {
        void DisableControlAction(InputGroup inputGroup, Enums.Control control, bool disable = true);
        float GetDisabledControlNormal(InputGroup inputGroup, Enums.Control control);
        void DisableAllControlActions(InputGroup inputGroup);
        bool IsControlPressed(InputGroup inputGroup, Enums.Control control);
        bool IsDisabledControlPressed(InputGroup inputGroup, Enums.Control control);
        /**
         * <summary>
         * This is for simulating player input. amount is a float value from 0 - 1 0, 1
         * and 2 used in the scripts. 0 is by far the most common of them.
         * </summary>
         * */
        bool SetControlNormal(InputGroup inputGroup, Enums.Control control, float amount);
        bool IsDisabledControlJustReleased(InputGroup inputGroup, Enums.Control control);
        bool IsControlJustPressed(InputGroup inputGroup, Enums.Control control);
        bool IsControlJustReleased(InputGroup inputGroup, Enums.Control control);
        bool IsDisabledControlJustPressed(InputGroup inputGroup, Enums.Control control);
    }
}
