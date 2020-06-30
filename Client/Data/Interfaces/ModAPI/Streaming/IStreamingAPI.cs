using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Streaming
{
    public interface IStreamingAPI
    {
        #region Public Methods

        void ClearFocus();

        bool HasModelLoaded(uint model);

        bool HasNamedPtfxAssetLoaded(string fxName);

        bool IsModelInCdimage(uint model);

        bool IsModelValid(uint model);

        void RequestAnimDict(string animDict);

        void RequestModel(uint model);

        void RequestNamedPtfxAsset(string fxName);

        void SetFocusArea(Position3D pos, int offsetX, int offsetY, int offsetZ);

        void SetFocusEntity(IEntityBase entity);

        #endregion Public Methods
    }
}
