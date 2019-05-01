using System.Collections.Generic;

namespace TDS_Server.server.instance.lobby.ganglobby
{
    partial class Gang
    {
        private List<string> rankNames = new List<string> { "Member", "Leader" };

        public void ChangeRanks(string[] ranks)
        {
            if (ranks.Length != rankNames.Count)
            {
                //ResetRights ( (uint) ranks.Length );
            }

            rankNames = new List<string>(ranks);
        }
    }
}