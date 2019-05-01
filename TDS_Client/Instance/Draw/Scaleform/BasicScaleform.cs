using RAGE.Game;

namespace TDS_Client.Instance.Draw.Scaleform
{
    class BasicScaleform
    {
        private int handle;

        public BasicScaleform(string scaleformName) => handle = Graphics.RequestScaleformMovie(scaleformName);

        public void Call(string functionName, params object[] args)
        {
            Graphics.PushScaleformMovieFunction(handle, functionName);
            foreach (object arg in args)
            {
                if (arg is string)
                    Graphics.PushScaleformMovieFunctionParameterString((string)arg);
                else if (arg is bool)
                    Graphics.PushScaleformMovieFunctionParameterBool((bool)arg);
                else if (arg is int)
                    Graphics.PushScaleformMovieFunctionParameterInt((int)arg);
                else if (arg is float)
                    Graphics.PushScaleformMovieFunctionParameterFloat((float)arg);
            }
            Graphics.PopScaleformMovieFunctionVoid();
        }

        public void RenderFullscreen()
        {
            Graphics.DrawScaleformMovieFullscreen(handle, 255, 255, 255, 255, 0);
        }

        public void Destroy()
        {
            Graphics.SetScaleformMovieAsNoLongerNeeded(ref handle);
        }
    }
}
