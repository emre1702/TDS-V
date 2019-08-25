using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;

namespace TDS_Client.Instance.Draw.Scaleform
{
    internal class BasicScaleform : IDisposable
    {
        private bool IsLoaded => Graphics.HasScaleformMovieLoaded(_handle);
        private bool IsValid => _handle != 0;

        private Queue<(string, object[])> _functionQueue = new Queue<(string, object[])>();
        private int _handle;

        public BasicScaleform(string scaleformName)
        {
            _handle = Graphics.RequestScaleformMovie(scaleformName);
        }

        public void Call(string functionName, params object[] args)
        {
            if (!IsLoaded || !IsValid)
            {
                _functionQueue.Enqueue((functionName, args));
                return;
            }
            Graphics.PushScaleformMovieFunction(_handle, functionName);
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
            OnUpdate();
            if (IsLoaded && IsValid)
                Graphics.DrawScaleformMovieFullscreen(_handle, 255, 255, 255, 255, 0);
        }

        public void Render2D(float x, float y, float width, float height)
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                Graphics.DrawScaleformMovie(_handle, x, y, width, height, 255, 255, 255, 255, 0);
        }

        public void Render3D(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                Graphics.DrawScaleformMovie3dNonAdditive(_handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2, 2, 1, scale.X, scale.Z, scale.Z, 2);
        }

        public void Render3DAdditive(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                Graphics.DrawScaleformMovie3d(_handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2, 2, 1, scale.X, scale.Z, scale.Z, 2);
        }

        private void OnUpdate()
        {
            if (_functionQueue.Count == 0 || !IsLoaded || !IsValid)
                return;
            foreach (var entry in _functionQueue)
            {
                Call(entry.Item1, entry.Item2);
            }
            _functionQueue.Clear();
        }

        #region IDisposable Support
        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Graphics.SetScaleformMovieAsNoLongerNeeded(ref _handle);
                    _functionQueue = null;
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}