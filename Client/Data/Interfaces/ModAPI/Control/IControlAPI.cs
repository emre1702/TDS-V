using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Control
{
    public interface IControlAPI
    {
        void DisableControlAction(InputGroup inputGroup, Enums.Control control, bool disable = true);
        float GetDisabledControlNormal(InputGroup inputGroup, Enums.Control control);
    }
}
