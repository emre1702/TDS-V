﻿namespace TDS.server.manager.player {

	using System.Collections.Generic;
	using database;
	using GTANetworkAPI;
    using TDS.server.extend;
    using utility;

	static class Register {

		public static void RegisterPlayer ( Client player, uint uid, string password, string email ) {
			Dictionary<string, string> parameters = new Dictionary<string, string> {
                {
					"@name", player.SocialClubName
				}, {
					"@password", Utility.ConvertToSHA512 ( password )
				}, {
					"@email", email
				}
			};
			Database.ExecPrepared ( $"INSERT INTO player (uid, name, password, email, registerdate) VALUES ({uid}, @name, @password, @email, '{Utility.GetTimestamp ()}');", parameters );
            Database.Exec ( $"INSERT INTO playerarenastats (uid) VALUES ({uid});" );
            Account.AddAccount ( player.SocialClubName, uid );

			logs.Log.Register ( player );

            Login.LoginPlayer ( player.GetChar(), uid );
        }
	}

}