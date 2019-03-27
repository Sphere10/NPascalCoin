using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sphere10.Framework;

using  NPascalCoin.Common;

namespace NPascalCoin.Common.Text {
	public static class Extensions {

		public static EPasa Parse(this IEPasaParser parser, string epasaText) {
			if (!parser.TryParse(epasaText, out var epasa, out var errorCode))
				throw new FormatException($"Invalid EPASA '{epasaText}': {errorCode}");
			return epasa;
		}

		public static bool TryParse(this IEPasaParser parser, string epasaText, out EPasa epasa) {
			return parser.TryParse(epasaText, out epasa, out var errorCode);
		}


	}

}
