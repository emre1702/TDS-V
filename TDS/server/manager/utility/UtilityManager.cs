using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Shared;


namespace Manager {
	class Utility : Script {
		public static readonly Random rnd = new Random ();
		private static DateTime startDateTime = new DateTime ( 2017, 7, 24 );

		public static string ConvertToSHA512 ( string input ) {
			byte[] hashbytes = SHA512Managed.Create ().ComputeHash ( Encoding.Default.GetBytes ( input ) );
			StringBuilder sb = new StringBuilder ();
			for ( int i = 0; hashbytes != null && i < hashbytes.Length; i++ ) {
				sb.AppendFormat ( "{0:x2}", hashbytes[i] );
			}
			return sb.ToString ();
		}

		public static int GetTimespan ( int seconds = 0 ) {
			TimeSpan t = DateTime.Now.AddSeconds ( seconds ) - startDateTime;
			return (int) t.TotalSeconds;
		}

		public static string GetTimestamp ( int seconds = 0 ) {
			return DateTime.Now.AddSeconds ( seconds ).ToString ( "de-DE" );
		}

	}
}
