using GrandTheftMultiplayer.Server.API;

namespace Manager {
	class ResourceStop : Script {
		public ResourceStop ( ) {
			Manager.Log.SaveInDatabase ();
		}
	}
}