using RAGE;
using System;
using System.Collections.Generic;
using TDS.Client.Data.Models;
using TDS.Shared.Core;
using static RAGE.Events;

namespace TDS.Client.Handler.Draw.Dx
{
    public class DxHandler : ServiceBase
    {
        public int ResX;
        public int ResY;

        private readonly List<DxBase> _dxDraws = new List<DxBase>();

        public DxHandler(LoggingHandler loggingHandler) : base(loggingHandler)
        {
            Tick += RenderAll;
            RefreshResolution();
            new TDSTimer(RefreshResolution, 10000, 0);
        }

        public void Add(DxBase dx)
        {
            _dxDraws.Add(dx);
            _dxDraws.Sort((a, b) => a.FrontPriority.CompareTo(b.FrontPriority));
        }

        public void RefreshResolution()
        {
            RAGE.Game.Graphics.GetScreenResolution(ref ResX, ref ResY);
        }

        public void Remove(DxBase dxBase)
        {
            _dxDraws.Remove(dxBase);
        }

        public void RenderAll(List<TickNametagData> _)
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
    }
}
