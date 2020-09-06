using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Data.Enums;
using Alignment = RAGE.NUI.UIResText.Alignment;

namespace TDS_Client.Handler.Draw.Dx.Grid
{
    public class DxGrid : DxBase
    {
        public readonly List<DxGridColumn> Columns = new List<DxGridColumn>();
        public Alignment Alignment;
        public float RowHeight;
        public int ScrollIndex;
        public float X, Y, Width, BodyHeight;

        private readonly List<DxGridRow> _rows = new List<DxGridRow>();
        private Color _bodyBackColor;
        private Font _bodyFont;
        private float _bodyTextScale;
        private int _maxRows;

        public DxGrid(DxHandler dxHandler, float x, float y, float width, float bodyHeight, Color bodyBackColor, float bodyTextScale = 1.0f, Font bodyFont = Font.ChaletLondon,
            Alignment alignment = Alignment.Centered, int maxRows = 25, int frontPriority = 0) : base(dxHandler, frontPriority)
        {
            X = x;
            Y = y;
            Width = width;
            BodyHeight = bodyHeight;
            _bodyBackColor = bodyBackColor;
            _bodyTextScale = bodyTextScale;
            _bodyFont = bodyFont;
            Alignment = alignment;
            _maxRows = maxRows;

            RowHeight = BodyHeight / maxRows;
        }

        public DxGridRow Header { get; private set; }

        public void AddRow(DxGridRow row, bool setPriority = true)
        {
            _rows.Add(row);
            row.Grid = this;
            if (setPriority)
                row.FrontPriority = FrontPriority + 1;
            Children.Add(row);
        }

        public void ClearRows()
        {
            foreach (var row in _rows)
                row.Remove();
            _rows.Clear();
        }

        public override void Draw()
        {
            CheckScroll();
            float atYTopPos = Y - BodyHeight / 2;
            if (Header != null && Header.Activated)
            {
                Header.Y = atYTopPos;
                atYTopPos = Header.Draw();
            }

            for (int i = 0; i < Math.Min(_rows.Count, _maxRows); ++i)
            {
                int index = i + ScrollIndex;
                DxGridRow row = _rows[index];
                row.Y = atYTopPos + row.Height / 2;
                atYTopPos = row.Draw();
            }
        }

        public override DxType GetDxType()
        {
            return DxType.Grid;
        }

        public void SetHeader(DxGridRow row, bool setPriority = true)
        {
            Header = row;
            row.Grid = this;
            if (setPriority)
                row.FrontPriority = FrontPriority + 1;
            Children.Add(row);
        }

        private void CheckScroll()
        {
            int rowscount = _rows.Count;
            if (rowscount <= _maxRows)
            {
                ScrollIndex = 0;
                return;
            }
            int change = (int)(Math.Ceiling((double)rowscount - _maxRows) / 10);
            if (Pad.IsControlJustPressed((int)InputGroup.MOVE, (int)Control.SelectNextWeapon))
            {
                if (ScrollIndex + change < rowscount - _maxRows)
                    ScrollIndex += change;
                else
                    ScrollIndex = rowscount - _maxRows;
            }
            else if (Pad.IsControlJustPressed((int)InputGroup.MOVE, (int)Control.SelectPrevWeapon))
            {
                if (ScrollIndex - change > 0)
                    ScrollIndex -= change;
                else
                    ScrollIndex = 0;
            }
        }
    }
}
