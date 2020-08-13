namespace TDS_Server.Entity.LobbySystem.GangLobbySystem
{
    partial class GangLobby
    {
        #region Private Methods

        private void LoadTeams()
        {
            foreach (var team in Teams)
            {
                var teamId = team.Entity.Id;
                var gang = _gangsHandler.GetByTeamId(teamId);
                if (gang != null)
                {
                    gang.GangLobbyTeam = team;
                }
            }
        }

        #endregion Private Methods

        /*private Dictionary<Gang, int> gangTeamID = new Dictionary<Gang, int> ();

        public void SetPlayerTeam ( Character character, Gang gang ) {
            SetPlayerTeam ( character, GetGangTeamID ( gang ) );
        }

        private int GetGangTeamID ( Gang gang ) {
            if ( gangTeamID.ContainsKey ( gang ) )
                return gangTeamID[gang];
            int id = Teams.Count;

            return id;
        }*/
    }
}
