using System;
using System.Collections.Generic;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Entities.Draw.Scaleform
{
    internal class BasicScaleform : IDisposable
    {
        private bool IsLoaded => _modAPI.Graphics.HasScaleformMovieLoaded(_handle);
        private bool IsValid => _handle != 0;

        private Queue<(string, object[])> _functionQueue = new Queue<(string, object[])>();
        private int _handle;

        private readonly IModAPI _modAPI;

        public BasicScaleform(string scaleformName, IModAPI modAPI)
        {
            _modAPI = modAPI;

            _handle = modAPI.Graphics.RequestScaleformMovie(scaleformName);
        }

        public void Call(string functionName, params object[] args)
        {
            if (!IsLoaded || !IsValid)
            {
                _functionQueue.Enqueue((functionName, args));
                return;
            }
            _modAPI.Graphics.PushScaleformMovieFunction(_handle, functionName);
            foreach (object arg in args)
            {
                if (arg is string)
                    _modAPI.Graphics.PushScaleformMovieFunctionParameterString((string)arg);
                else if (arg is bool)
                    _modAPI.Graphics.PushScaleformMovieFunctionParameterBool((bool)arg);
                else if (arg is int)
                    _modAPI.Graphics.PushScaleformMovieFunctionParameterInt((int)arg);
                else if (arg is float)
                    _modAPI.Graphics.PushScaleformMovieFunctionParameterFloat((float)arg);
            }
            _modAPI.Graphics.PopScaleformMovieFunctionVoid();
        }


        public void RenderFullscreen()
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                _modAPI.Graphics.DrawScaleformMovieFullscreen(_handle, 255, 255, 255, 255, 0);
        }

        public void Render2D(float x, float y, float width, float height)
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                _modAPI.Graphics.DrawScaleformMovie(_handle, x, y, width, height, 255, 255, 255, 255, 0);
        }

        public void Render3D(Position3D position, Position3D rotation, Position3D scale)
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                _modAPI.Graphics.DrawScaleformMovie3dNonAdditive(_handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2, 2, 1, scale.X, scale.Z, scale.Z, 2);
        }

        public void Render3DAdditive(Position3D position, Position3D rotation, Position3D scale)
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                _modAPI.Graphics.DrawScaleformMovie3d(_handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2, 2, 1, scale.X, scale.Z, scale.Z, 2);
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
                    _modAPI.Graphics.SetScaleformMovieAsNoLongerNeeded(ref _handle);
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
