using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.RAGE.Game.Entity;

namespace TDS_Client.RAGEAPI.Cam
{
    internal class Camera : RAGE.Elements.Camera, IEntity
    {
        #region Public Constructors

        public Camera(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public new EntityType Type => EntityType.Camera;

        #endregion Public Properties

        #region Public Methods

        public bool Equals(IEntity other)
            => Id == other?.Id;

        #endregion Public Methods
    }
}