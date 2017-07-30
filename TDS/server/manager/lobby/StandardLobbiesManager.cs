using GrandTheftMultiplayer.Server.API;

namespace Manager {
	static class StandardLobbies {
		public static void CreateStandardLobbies ( ) {
			Arena.Create ();
			MainMenu.Create ();
		}
	}
}
