using TDS_Client.Data.Enums;
using TDS_Client.Handler.Draw;

namespace TDS_Client.Handler.Entities.Draw.Scaleform
{
    public class InstructionalButton
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

        public string OriginalControlString { get; set; }


        private string _title;
        private int _slot;
        private string _controlString;
        private Control _controlEnum;
        private bool _useStringForControl;

        private readonly InstructionalButtonHandler _instructionalButtonHandler;

        private InstructionalButton(string title, int slot, InstructionalButtonHandler instructionalButtonHandler)
        {
            _title = title;
            _slot = slot;
            _instructionalButtonHandler = instructionalButtonHandler;
        }

        public InstructionalButton(string title, Control control, int slot, InstructionalButtonHandler instructionalButtonHandler) 
            : this(title, slot, instructionalButtonHandler)
        {
            _controlEnum = control;
            _useStringForControl = false;
            instructionalButtonHandler.SetDataSlot(_slot, _controlEnum, _title);
        }

        public InstructionalButton(string title, string control, int slot, InstructionalButtonHandler instructionalButtonHandler) 
            : this(title, slot, instructionalButtonHandler)
        {
            OriginalControlString = control;
            if (!control.StartsWith("t_") && !control.StartsWith("w_"))
            {
                if (control.Length <= 2)
                    control = "t_" + control;
                else
                    control = "w_" + control;
            }
            _controlString = control;
            _useStringForControl = true;
            instructionalButtonHandler.SetDataSlot(_slot, _controlString, _title);
        }

        public void SetTitle(string title)
        {
            _title = title;
            if (_useStringForControl)
                _instructionalButtonHandler.SetDataSlot(_slot, _controlString, _title);
            else
                _instructionalButtonHandler.SetDataSlot(_slot, _controlEnum, _title);
        }

        public void SetSlot(int slot)
        {
            _slot = slot;
            if (_useStringForControl)
                _instructionalButtonHandler.SetDataSlot(_slot, _controlString, _title);
            else
                _instructionalButtonHandler.SetDataSlot(_slot, _controlEnum, _title);
        }

        public void SetControl(Control control)
        {
            _controlEnum = control;
            _useStringForControl = false;
            _instructionalButtonHandler.SetDataSlot(_slot, _controlEnum, _title);
        }

        public void SetControl(string control)
        {
            if (!control.StartsWith("t_"))
                control = "t_" + control;
            _controlString = control;
            _useStringForControl = true;
            _instructionalButtonHandler.SetDataSlot(_slot, _controlString, _title);
        }
    }
}
