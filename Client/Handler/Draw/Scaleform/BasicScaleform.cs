using System;
using System.Collections.Generic;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Entities.Draw.Scaleform
{
    internal class BasicScaleform : IDisposable
    {
        private bool _disposedValue = false;
        private Queue<(string, object[])> _functionQueue = new Queue<(string, object[])>();
        private int _handle;

        public BasicScaleform(string scaleformName)
            => _handle = RAGE.Game.Graphics.RequestScaleformMovie(scaleformName);

        private bool IsLoaded => RAGE.Game.Graphics.HasScaleformMovieLoaded(_handle);
        private bool IsValid => _handle != 0;

        public void Call(string functionName, params object[] args)
        {
            if (!IsLoaded || !IsValid)
            {
                _functionQueue.Enqueue((functionName, args));
                return;
            }
            RAGE.Game.Graphics.PushScaleformMovieFunction(_handle, functionName);
            foreach (object arg in args)
            {
                if (arg is string @string)
                    RAGE.Game.Graphics.PushScaleformMovieFunctionParameterString(@string);
                else if (arg is bool @bool)
                    RAGE.Game.Graphics.PushScaleformMovieFunctionParameterBool(@bool);
                else if (arg is int @int)
                    RAGE.Game.Graphics.PushScaleformMovieFunctionParameterInt(@int);
                else if (arg is float @float)
                    RAGE.Game.Graphics.PushScaleformMovieFunctionParameterFloat(@float);
            }
            RAGE.Game.Graphics.PopScaleformMovieFunctionVoid();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Render2D(float x, float y, float width, float height)
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                RAGE.Game.Graphics.DrawScaleformMovie(_handle, x, y, width, height, 255, 255, 255, 255, 0);
        }

        public void Render3D(Position3D position, Position3D rotation, Position3D scale)
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                RAGE.Game.Graphics.DrawScaleformMovie3dNonAdditive(_handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2, 2, 1, scale.X, scale.Z, scale.Z, 2);
        }

        public void Render3DAdditive(Position3D position, Position3D rotation, Position3D scale)
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                RAGE.Game.Graphics.DrawScaleformMovie3d(_handle, position.X, position.Y, position.Z, rotation.X, rotation.Y, rotation.Z, 2, 2, 1, scale.X, scale.Z, scale.Z, 2);
        }

        public void RenderFullscreen()
        {
            OnUpdate();
            if (IsLoaded && IsValid)
                RAGE.Game.Graphics.DrawScaleformMovieFullscreen(_handle, 255, 255, 255, 255, 0);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    RAGE.Game.Graphics.SetScaleformMovieAsNoLongerNeeded(ref _handle);
                    _functionQueue = null;
                }

                _disposedValue = true;
            }
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
    }
}
