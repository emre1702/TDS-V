using TDS_Client.Data.Interfaces.ModAPI.Nametags;

namespace TDS_Client.RAGEAPI.Nametags
{
    class NametagsAPI : INametagsAPI
    {
        public bool Enabled 
        { 
            get => RAGE.Nametags.Enabled; 
            set => RAGE.Nametags.Enabled = value; 
        }
    }
}
