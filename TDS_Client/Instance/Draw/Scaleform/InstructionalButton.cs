using RAGE.Game;
using System.Drawing;
using TDS_Client.Manager.Draw;

namespace TDS_Client.Instance.Draw.Scaleform
{
    class InstructionalButton
    {
        public string Title
        {
            get => _title;
            set => SetTitle(value);
        }
        public int Slot
        {
            get => _slot;
            set => SetSlot(value);
        }
        public string ControlString
        {
            get => _controlString;
            set => SetControl(value);
        }
        public Control ControlEnum
        {
            get => _controlEnum;
            set => SetControl(value);
        }
        
        private string _title;
        private int _slot;
        private string _controlString;
        private Control _controlEnum;
        private bool _useStringForControl;

        private InstructionalButton(string title, int slot)
        {
            _title = title;
            _slot = slot;
        }

        public InstructionalButton(string title, Control control, int slot) : this(title, slot)
        {
            _controlEnum = control;
            _useStringForControl = false;
            InstructionalButtonManager.SetDataSlot(_slot, _controlEnum, _title);
        }

        public InstructionalButton(string title, string control, int slot) : this(title, slot)
        {
            if (!control.StartsWith("t_") && !control.StartsWith("w_"))
            {
                if (control.Length <= 2)
                    control = "t_" + control;
                else
                    control = "w_" + control;
            }
            _controlString = control;
            _useStringForControl = true;
            InstructionalButtonManager.SetDataSlot(_slot, _controlString, _title);
        }

        public void SetTitle(string title)
        {
            _title = title;
            if (_useStringForControl)
                InstructionalButtonManager.SetDataSlot(_slot, _controlString, _title);
            else
                InstructionalButtonManager.SetDataSlot(_slot, _controlEnum, _title);
        }

        public void SetSlot(int slot)
        {
            _slot = slot;
            if (_useStringForControl)
                InstructionalButtonManager.SetDataSlot(_slot, _controlString, _title);
            else
                InstructionalButtonManager.SetDataSlot(_slot, _controlEnum, _title);
        }

        public void SetControl(Control control)
        {
            _controlEnum = control;
            _useStringForControl = false;
            InstructionalButtonManager.SetDataSlot(_slot, _controlEnum, _title);
        }

        public void SetControl(string control)
        {
            if (!control.StartsWith("t_"))
                control = "t_" + control;
            _controlString = control;
            _useStringForControl = true;
            InstructionalButtonManager.SetDataSlot(_slot, _controlString, _title);
        }
    }
}
