using System;
using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Input
{
    public interface IInputAPI
    {
        #region Public Methods

        int Bind(Key keyCode, bool down, Action handler);

        int Bind(int keyCode, bool down, Action handler);

        bool IsDown(int keyCode);

        bool IsDown(Key keyCode);

        bool IsUp(int keyCode);

        bool IsUp(Key keyCode);

        void TakeScreenshot(string name, int type, float quality, float compQuality);

        void Unbind(int keyCode, bool down, Action handler);

        void Unbind(int keyCode, bool down, int bindIdx);

        void Unbind(Key keyCode, bool down, Action handler);

        void Unbind(Key keyCode, bool down, int bindIdx);

        #endregion Public Methods
    }
}
