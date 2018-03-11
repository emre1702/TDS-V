namespace TDS.server.manager.utility {

	using System.Collections.Generic;

	static class Money {

		public static Dictionary<string, double> MoneyForDict = new Dictionary<string, double> {
			{ "kill", 20 },
			{ "assist", 10 },
			{ "damage", 0.1	}
		};
	}

}
