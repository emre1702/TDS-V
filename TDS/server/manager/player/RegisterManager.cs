﻿using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Manager {
	static class Register {

		public static async Task RegisterPlayer ( Client player, int uid, string password, string email ) {
			Dictionary<string, string> parameters = new Dictionary<string, string> {
				{ "@UID", uid.ToString() },
				{ "@name", player.socialClubName },
				{ "@password", Utility.ConvertToSHA512 ( password ) },
				{ "@email", email },
				{ "@registerdate", Utility.GetTimestamp() }
			};
			Dictionary<string, string> defaultparams = new Dictionary<string, string> { { "@UID", uid.ToString () } };
			await Database.ExecPrepared ( "INSERT INTO player (UID, name, password, email, registerdate) VALUES (@UID, @name, @password, @email, @registerdate);", parameters ).ConfigureAwait ( false );
			await Database.ExecPrepared ( "INSERT INTO playersetting (UID) VALUES (@UID)", defaultparams ).ConfigureAwait ( false );
			Account.AddAccount ( player.socialClubName, uid );
			Login.LoginPlayer ( player, uid );
		}
	}
}
