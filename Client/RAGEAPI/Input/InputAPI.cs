using System;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Input;

namespace TDS_Client.RAGEAPI.Input
{
    class InputAPI : IInputAPI
    {
        public bool IsDown(int keyCode)
            => RAGE.Input.IsDown(keyCode);
        public bool IsDown(Key keyCode)
            => RAGE.Input.IsDown((RAGE.Ui.VirtualKeys)keyCode);

        public bool IsUp(int keyCode)
            => RAGE.Input.IsUp(keyCode);

        public bool IsUp(Key keyCode)
            => RAGE.Input.IsUp((RAGE.Ui.VirtualKeys)keyCode);

        public int Bind(Key keyCode, bool down, Action handler)
            => RAGE.Input.Bind((RAGE.Ui.VirtualKeys)keyCode, down, handler);

        public int Bind(int keyCode, bool down, Action handler)
            => RAGE.Input.Bind(keyCode, down, handler);

        public void Unbind(int keyCode, bool down, Action handler) 
            => RAGE.Input.Unbind(keyCode, down, handler);

        public void Unbind(int keyCode, bool down, int bindIdx)
            => RAGE.Input.Unbind(keyCode, down, bindIdx);
        
        public void Unbind(Key keyCode, bool down, Action handler)
            => RAGE.Input.Unbind((RAGE.Ui.VirtualKeys)keyCode, down, handler);

        public void Unbind(Key keyCode, bool down, int bindIdx)
            => RAGE.Input.Unbind((RAGE.Ui.VirtualKeys)keyCode, down, bindIdx);

        public void TakeScreenshot(string name, int type, float quality, float compQuality)
            => RAGE.Input.TakeScreenshot(name, type, quality, compQuality);
    }
}
