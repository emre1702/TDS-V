using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Streaming;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Streaming
{
    internal class StreamingAPI : IStreamingAPI
    {
        #region Public Methods

        public void ClearFocus()
            => RAGE.Game.Streaming.ClearFocus();

        public bool HasModelLoaded(uint model)
        {
            return RAGE.Game.Streaming.HasModelLoaded(model);
        }

        public bool IsModelInCdimage(uint model)
        {
            return RAGE.Game.Streaming.IsModelInCdimage(model);
        }

        public bool IsModelValid(uint model)
        {
            return RAGE.Game.Streaming.IsModelValid(model);
        }

        public void RequestAnimDict(string animDict)
        {
            RAGE.Game.Streaming.RequestAnimDict(animDict);
        }

        public void RequestModel(uint model)
        {
            RAGE.Game.Streaming.RequestModel(model);
        }

        public void RequestNamedPtfxAsset(string fxName)
            => RAGE.Game.Streaming.RequestNamedPtfxAsset(fxName);

        public void SetFocusArea(Position3D pos, int offsetX, int offsetY, int offsetZ)
        {
            RAGE.Game.Streaming.SetFocusArea(pos.X, pos.Y, pos.Z, offsetX, offsetY, offsetZ);
        }

        public void SetFocusEntity(IEntityBase entity)
        {
            RAGE.Game.Streaming.SetFocusEntity(entity.Handle);
        }

        #endregion Public Methods
    }
}
