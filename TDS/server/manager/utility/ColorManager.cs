using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;

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
		// doesnt work //
		 //private static readonly uint[] blipColors = {
			// 0
			//0xFFFFFF,

			// 1 - 23
			//0x000000/*0xE03A3A*/, 0x000000/*0x78CD78*/, 0x000000/*0x65B9E7*/, 0xF0F0F0, 0x000000/*0xEFCA57*/, 0xC55758, 0xA175B4, 0xFF80C8, 0xF6A480, 0xB6968B, 0x91CFAA, 0x77ACB2, 0xD5D3E8, 0x94849E, 0x70C8C2, 0xD8C69E, 0xEC9359, 0x9ECDEB, 0xB6698D, 0x95927F, 0xAA7B67, 0xB4ABAC, 0xE993A0,
			// 24 - 46
			//0xBFD863, 0x17815D, 0x80C7FF, 0xAF45E7, 0xCFAB17, 0x4F6AB1, 0x34A9BC, 0xBCA183, 0xCDE2FF, 0xF0F09A, 0xEE91A4, 0xF98E8E, 0xFDF0AA, 0xF1F1F1, 0x3776BD, 0x9F9F9F, 0x555555, 0xF29E9E, 0x6DB8D7, 0xAFEEAF, 0xFFA65F, 0xF0F0F0, 0xECF029,
			// 47 - 69
			//0xFE9917, 0xF745A5, 0xE03A3A, 0x8A6DE3, 0xFF8B5C, 0x426D42, 0xB3DDF3, 0x396479, 0xA0A0A0, 0x847232, 0x64B9E6, 0x4C4276, 0xE03B3B, 0xF0CB58, 0xCD3E98, 0xCFCFCF, 0x286B9F, 0xD87A1B, 0x8E8393, 0xF0CB58, 0x65B9E7, 0x64B8E6, 0x78CD78,
			// 70 - 85
			//0xEFCA57, 0xF0CB58, 0x000000, 0xEFCA57, 0x65B9E7, 0xE13B3B, 0x782424, 0x65B9E7, 0x396479, 0x000000/*0xE13B3B*/, 0x000000/*0x64B8E6*/, 0xF1A30B, 0xA4CBA9, 0xA753F1, 0x64B9E7, 0x000000
		//};

		/* private static Vector3 ParseColor ( string colorHex ) {
			return new Vector3 (
				int.Parse ( colorHex.Substring ( 0, 2 ), System.Globalization.NumberStyles.HexNumber ) / 255.0f,
				int.Parse ( colorHex.Substring ( 2, 2 ), System.Globalization.NumberStyles.HexNumber ) / 255.0f,
				int.Parse ( colorHex.Substring ( 4, 2 ), System.Globalization.NumberStyles.HexNumber ) / 255.0f
			);
		}

		private static Vector3 ParseColor ( uint color ) {
			return new Vector3 (
				( ( color & 0x00FF0000 ) >> 16 ) / 255.0f,
				( ( color & 0x0000FF00 ) >> 8 ) / 255.0f,
				( color & 0x000000FF ) / 255.0f
			);
		}

		public static int GetClosestBlipColor ( Vector3 color ) {
			float bestDiff = 255.0f * 3.0f;
			int ret = -1;

			for ( int i = 0; i < blipColors.Length; i++ ) {
				Vector3 col = ParseColor ( blipColors[i] );

				if ( ret == -1 ) {
					ret = i;
					continue;
				}

				float diffR = Math.Abs ( col.X - color.X );
				float diffG = Math.Abs ( col.Y - color.Y );
				float diffB = Math.Abs ( col.Z - color.Z );
				float diff = diffR + diffG + diffB;

				if ( diff < bestDiff ) {
					bestDiff = diff;
					ret = i;
				}
			}
			return ret;
		} */
	}
}