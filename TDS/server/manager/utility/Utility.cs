namespace TDS.server.manager.utility {

	using System;
	using System.Globalization;
	using System.Security.Cryptography;
	using System.Text;

	static class Utility {
		public static readonly Random Rnd = new Random ();
		private static readonly DateTime startDateTime = new DateTime ( 2017, 7, 24 );

		public enum AnimationFlags {
			Loop = 1 << 0,
			StopOnLastFrame = 1 << 1,
			OnlyAnimateUpperBody = 1 << 4,
			AllowPlayerControl = 1 << 5,
			Cancellable = 1 << 7
		}

		public static string ConvertToSHA512 ( string input ) {
			byte[] hashbytes = SHA512.Create ().ComputeHash ( Encoding.Default.GetBytes ( input ) );
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
			if ( seconds == 0 )
				return DateTime.UtcNow.AddHours ( 2 ).ToString ( "dd-MM-yyyy HH:mm:ss" );
			return DateTime.UtcNow.AddHours ( 2 ).AddSeconds ( seconds ).ToString ( "dd-MM-yyyy HH:mm:ss" );
		}

		public static float ToFloat ( this string str ) {
			return float.Parse ( str, CultureInfo.InvariantCulture );
		}

	}

}
