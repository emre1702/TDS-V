﻿namespace TDS_Server.server.instance.lobby.ganglobby
{
    partial class Gang
    {
        /*private Dictionary<GangActivity, uint> allowedAtRank = new Dictionary<GangActivity, uint>
		{
			{ GangActivity.INVITE, 2 },
			{ GangActivity.UNINVITE, 2 },
			{ GangActivity.RANKUP, 2 },
			{ GangActivity.RANKDOWN, 2 },
			{ GangActivity.CHANGE_RANKNAMES, 2 },
			{ GangActivity.CHANGE_RANKCOLORS, 2 }
		};

		public bool IsAllowedTo ( Character character, GangActivity activity ) {
			if ( IsOwner ( character ) )
				return true;

			if ( allowedAtRank.ContainsKey ( activity ) )
				return IsAtleastRank ( character, allowedAtRank[activity] );

			Log.Error ( $"GangActivity {activity.ToString ()} is not included in allowedAtRank!", "GANG" );
			return false;
		}

		public bool IsOwner ( Character character ) {
			return character.UID == owneruid;
		}

		public bool IsAtleastRank ( Character character, uint rank ) {
			return character.GangRank >= rank;
		}

		public void ResetRights ( uint tonumber ) {
			foreach ( GangActivity key in allowedAtRank.Keys.ToList() ) {
				allowedAtRank[key] = tonumber;
			}
		}*/
    }
}