﻿using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler.Draw.Dx.Grid
{
    public class DxGridCell : DxBase
    {
        #region Public Fields

        public DxGridRow Row;

        #endregion Public Fields

        #region Private Fields

        private readonly AlignmentX _alignmentX;
        private Color? _backColor;
        private DxGridColumn _column;
        private DxText _dxText;
        private Font? _font;
        private float? _scale;
        private string _text;
        private Color? _textColor;

        #endregion Private Fields

        #region Public Constructors

        public DxGridCell(DxHandler dxHandler, IModAPI modAPI, TimerHandler timerHandler, string text, DxGridRow row, DxGridColumn column, Color? backColor = null,
            Color? textColor = null, float? scale = null, Font? font = null,
            AlignmentX? alignment = null, int frontPriority = 0) : base(dxHandler, modAPI, frontPriority, false)
        {
            this._text = text;
            this.Row = row;
            this._column = column;
            this._backColor = backColor;
            this._textColor = textColor;
            this._scale = scale;
            this._font = font;
            _alignmentX = alignment ?? row.TextAlignment;

            _dxText = new DxText(dxHandler, modAPI, timerHandler, text, column.X, Row.Y, scale ?? Row.Scale, textColor ?? Row.TextColor, font ?? Row.Font,
                alignment ?? Row.TextAlignment, AlignmentY.Center, _column.RelativePos, false, true, amountLines: 1, frontPriority: 99)
            {
                Activated = false
            };

            row.AddCell(this);
        }

        #endregion Public Constructors

        #region Public Properties

        public Color BackColor
        {
            get => _backColor ?? Row.BackColor;
            set
            {
                if (value != null && value != Row.BackColor)
                    Row.UseColorForWholeRow = false;
                else
                    Row.CheckUseColorForWholeRow();
            }
        }

        #endregion Public Properties

        #region Public Methods

        public override void Draw()
        {
            var x = GetXPos();
            if (_column.RelativePos)
                _dxText.SetRelativeX(x);
            else
                _dxText.SetAbsoluteX((int)x);

            if (Row.RelativePos)
                _dxText.SetRelativeY(Row.Y);
            else
                _dxText.SetAbsoluteY((int)Row.Y);
            _dxText.Draw();
        }

        public void DrawBackground()
        {
            Color backcolor = BackColor;
            ModAPI.Graphics.DrawRect(_column.X, Row.Y, _column.Width, Row.Height, backcolor.R, backcolor.G, backcolor.B, backcolor.A);
        }

        public override DxType GetDxType()
        {
            return DxType.GridCell;
        }

        public override void Remove()
        {
            base.Remove();
            _dxText.Remove();
            _dxText = null;
        }

        public void SetText(string text)
        {
            this._text = text;
        }

        #endregion Public Methods

        #region Private Methods

        private float GetXPos()
        {
            switch (_alignmentX)
            {
                case AlignmentX.Left:
                    return _column.X;

                case AlignmentX.Center:
                    return _column.X + _column.Width / 2;

                case AlignmentX.Right:
                    return _column.X + _column.Width;
            }
            return 0;
        }

        #endregion Private Methods
    }
}
