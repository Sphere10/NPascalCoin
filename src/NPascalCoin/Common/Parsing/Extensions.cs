using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sphere10.Framework;

using  NPascalCoin.Common;

namespace NPascalCoin.Common.Parsing {
	public static class Extensions {

		public static bool TryParse(this IEPasaParser parser, string epasaText, out EPasa epasa) {
			return parser.TryParse(epasaText, out epasa, out var errorCode);
		}


	}

}
