using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Constant;

namespace Manager {
	static class Colors {
		public static Dictionary<string, Color> fontColor = new Dictionary<string, Color> {
			{ "r", new Color ( 222, 50, 50 ) },
			{ "b", new Color ( 92, 180, 227 ) },
			{ "g", new Color ( 113, 202, 113 ) },
			{ "y", new Color ( 238, 198, 80 ) },
			{ "p", new Color ( 131, 101, 224 ) },
			{ "q", new Color ( 226, 79, 128 ) },
			{ "o", new Color ( 253, 132, 85 ) },
			{ "c", new Color ( 139, 139, 139 ) },
			{ "m", new Color ( 99, 99, 99 ) },
			{ "u", new Color ( 0, 0, 0 ) },
			// { "n", }, New Line
			{ "s", new Color ( 220, 220, 220 ) }, // unsicher
			{ "w", new Color ( 255, 255, 255 ) },  // unsicher 
			// { "h", }, Bold
		};
		// unsicher (alles)
		public static Dictionary<string, int> blipColorByString = new Dictionary<string, int> {
			{ "r", 49 },
			{ "b", 38 },
			{ "g", 24 },
			{ "y", 46 },
			{ "p", 7 },
			{ "q", 8 },
			{ "o", 47 },
			{ "c", 62 },
			{ "m", 39 },
			{ "u", 85 },
			{ "s", 4 },
			{ "w", 37 }
		};
	}
}