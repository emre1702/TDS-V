using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Manager {
	static class Register {

		public static void RegisterPlayer ( Client player, int uid, string password, string email ) {
			Dictionary<string, string> parameters = new Dictionary<string, string> {
				{ "@UID", uid.ToString() },
				{ "@name", player.socialClubName },
				{ "@password", Utility.ConvertToSHA512 ( password ) },
				{ "@email", email }
			};
			Database.ExecPrepared ( "INSERT INTO player (UID, name, password, email) VALUES (@UID, @name, @password, @email);", parameters );
			Account.AddAccount ( player.socialClubName, uid );
			System.Threading.Tasks.Task.Run ( ( ) => Login.LoginPlayer ( player, uid ) );
		}
	}
}
