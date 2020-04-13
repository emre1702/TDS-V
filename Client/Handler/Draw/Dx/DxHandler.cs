using System;
using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;

namespace TDS_Client.Handler.Draw.Dx
{
    public class DxHandler
    {
        public int ResX;
        public int ResY;

        private readonly List<DxBase> _dxDraws = new List<DxBase>();

        private readonly IModAPI _modAPI;

        public DxHandler(IModAPI modAPI)
        {
            _modAPI = modAPI;

            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(RenderAll));
            RefreshResolution();
        }

        public void RenderAll(int currentMs)
        {
            foreach (var draw in _dxDraws)
            {
                if (draw.Activated)
                    draw.Draw();
            }
        }

        public void RefreshResolution()
        {
            _modAPI.Graphics.GetScreenResolution(ref ResX, ref ResY);
        }

        public void Add(DxBase dx)
        {
            _dxDraws.Add(dx);
            _dxDraws.Sort((a, b) => a.FrontPriority.CompareTo(b.FrontPriority));
        }

        public void Remove(DxBase dxBase)
        {
            _dxDraws.Remove(dxBase);
        }
    }
}
