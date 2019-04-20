using RAGE.Game;
using RAGE.NUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Enum;

namespace TDS_Client.Instance.Draw.Dx.Grid
{
    class DxGrid : Dx
    {
        public readonly List<DxGridColumn> Columns = new List<DxGridColumn>();
        private readonly List<DxGridRow> rows = new List<DxGridRow>();
        public int ScrollIndex;

        public float X, Y, Width, BodyHeight;
        public UIResText.Alignment Alignment;
        public float RowHeight;

        public DxGridRow? Header { get; private set; }

        private float bodyTextScale;
        private Color bodyBackColor;
        private Font bodyFont;
        private int maxRows;
        

        public DxGrid(float x, float y, float width, float bodyHeight, Color bodyBackColor, float bodyTextScale = 1.0f, Font bodyFont = Font.ChaletLondon,
            UIResText.Alignment alignment = UIResText.Alignment.Centered, int maxRows = 25) : base()
        {
            X = x;
            Y = y;
            Width = width;
            BodyHeight = bodyHeight;
            this.bodyBackColor = bodyBackColor;
            this.bodyTextScale = bodyTextScale;
            this.bodyFont = bodyFont;
            this.Alignment = alignment;
            this.maxRows = maxRows;

            RowHeight = BodyHeight / maxRows;
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
            
            for (int i = 0; i < Math.Min(rows.Count, maxRows); ++i)
            {
                int index = i + ScrollIndex;
                DxGridRow row = rows[index];
                row.Y = atYTopPos + row.Height / 2;
                atYTopPos = row.Draw();
            }
        }

        public void SetHeader(DxGridRow row)
        {
            Header = row;
            row.Grid = this;
            children.Add(row);
        }

        public void AddRow(DxGridRow row)
        {
            rows.Add(row);
            row.Grid = this;
            children.Add(row);
        }

        public void AddCell(DxGridCell cell)
        {
            cell.Row.AddCell(cell);
            children.Add(cell);
        }

        public void ClearRows()
        {
            foreach (var row in rows)
                row.Remove();
            rows.Clear();
        }

        private void CheckScroll()
        {
            int rowscount = rows.Count;
            if (rowscount <= maxRows)
            {
                ScrollIndex = 0;
                return;
            }
            int change = (int)(Math.Ceiling((double)rowscount - maxRows) / 10);
            if (Pad.IsControlJustPressed(0, (int)Control.SelectNextWeapon))
            {
                if (ScrollIndex + change < rowscount - maxRows)
                    ScrollIndex += change;
                else
                    ScrollIndex = rowscount - maxRows;
            } 
            else if (Pad.IsControlJustPressed(0, (int)Control.SelectPrevWeapon))
            {

                if (ScrollIndex - change > 0)
                    ScrollIndex -= change;
                else
                    ScrollIndex = 0;
            }

        }

        public override EDxType GetDxType()
        {
            return EDxType.Grid;
        }
    }
}
