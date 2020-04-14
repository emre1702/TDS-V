using System;
using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Models;

namespace TDS_Client.Handler.Draw.Dx
{
    public class DxHandler : ServiceBase
    {
        public int ResX;
        public int ResY;

        private readonly List<DxBase> _dxDraws = new List<DxBase>();

        public DxHandler(IModAPI modAPI, LoggingHandler loggingHandler) : base(modAPI, loggingHandler)
        {
            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(RenderAll));
            RefreshResolution();
        }

        public void RenderAll(int currentMs)
        {
            try
            {
                foreach (var draw in _dxDraws)
                {
                    if (draw.Activated)
                        draw.Draw();
                }
            }
            catch (Exception ex)
            {
                Logging.LogError(ex);
            }
        }

        public void RefreshResolution()
        {
            ModAPI.Graphics.GetScreenResolution(ref ResX, ref ResY);
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
