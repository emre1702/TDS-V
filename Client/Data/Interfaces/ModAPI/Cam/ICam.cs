using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Cam
{
    public interface ICam
    {
        #region Public Properties

        Position3D Position { get; set; }
        Position3D Rotation { get; set; }

        #endregion Public Properties

        #region Public Methods

        void AttachTo(IEntityBase ped, PedBone bone, float x, float y, float z, bool heading);

        void Destroy();

        void Detach();

        void PointAtCoord(Position3D pos);

        void Render(bool render, bool ease, int easeTime);

        void SetActive(bool active);

        void SetFov(float fov);

        #endregion Public Methods
    }
}
