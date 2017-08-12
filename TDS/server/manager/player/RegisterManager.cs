using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Manager {
	static class Register {

		public static void RegisterPlayer ( Client player, int uid, string password, string email ) {
			Dictionary<string, string> parameters = new Dictionary<string, string> {
				{ "@UID", uid.ToString() },
				{ "@name", player.socialClubName },
				{ "@password", Utility.ConvertToSHA512 ( password ) },
				{ "@email", email }
			};
			Dictionary<string, string> defaultparams = new Dictionary<string, string> { { "@UID", uid.ToString () } };
			Task.Run ( ( ) => {
				Database.ExecPrepared ( "INSERT INTO player (UID, name, password, email) VALUES (@UID, @name, @password, @email);", parameters );
				Database.ExecPrepared ( "INSERT INTO playersetting (UID) VALUES (@UID)", defaultparams );
			} );
			Account.AddAccount ( player.socialClubName, uid );
			Task.Run ( ( ) => Login.LoginPlayer ( player, uid ) );
		}
	}
}
