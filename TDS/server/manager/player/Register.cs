namespace TDS.server.manager.player {

	using System.Collections.Generic;
	using System.Threading.Tasks;
	using database;
	using GTANetworkAPI;
	using utility;

	static class Register {

		public static async Task RegisterPlayer ( Client player, uint uid, string password, string email ) {
			Dictionary<string, string> parameters = new Dictionary<string, string> {
				{
					"@UID", uid.ToString ()
				}, {
					"@name", player.SocialClubName
				}, {
					"@password", Utility.ConvertToSHA512 ( password )
				}, {
					"@email", email
				}, {
					"@registerdate", Utility.GetTimestamp ()
				}
			};
			Dictionary<string, string> defaultparams = new Dictionary<string, string> {
				{
					"@UID", uid.ToString ()
				}
			};
			await Database.ExecPrepared ( "INSERT INTO player (UID, name, password, email, registerdate) VALUES (@UID, @name, @password, @email, @registerdate);", parameters ).ConfigureAwait ( false );
			await Database.ExecPrepared ( "INSERT INTO playersetting (UID) VALUES (@UID)", defaultparams ).ConfigureAwait ( false );
            NAPI.Task.Run ( ( ) => {
                Account.AddAccount ( player.SocialClubName, uid );
                Login.LoginPlayer ( player, uid );
            } );
        }
	}

}
