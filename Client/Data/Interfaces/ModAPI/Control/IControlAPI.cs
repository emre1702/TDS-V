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
        void SetControlNormal(InputGroup mOVE, Enums.Control attack, float v);
        bool IsDisabledControlJustReleased(InputGroup mOVE, Enums.Control attack);
        bool IsControlJustPressed(InputGroup mOVE, Enums.Control selectNextWeapon);
        bool IsControlJustReleased(InputGroup mOVE, Enums.Control cursorScrollUp);
        bool IsDisabledControlJustPressed(InputGroup mOVE, Enums.Control attack);
    }
}
