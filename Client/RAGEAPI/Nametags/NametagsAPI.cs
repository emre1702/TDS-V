using TDS_Client.Data.Interfaces.ModAPI.Nametags;

namespace TDS_Client.RAGEAPI.Nametags
{
    internal class NametagsAPI : INametagsAPI
    {
        #region Public Properties

        public bool Enabled
        {
            get => RAGE.Nametags.Enabled;
            set => RAGE.Nametags.Enabled = value;
        }

        #endregion Public Properties
    }
}
