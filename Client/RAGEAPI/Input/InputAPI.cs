using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Input;

namespace TDS_Client.RAGEAPI.Input
{
    class InputAPI : IInputAPI
    {
        public bool IsDown(Key key)
        {
            return RAGE.Input.IsDown((int)key);
        }
    }
}
