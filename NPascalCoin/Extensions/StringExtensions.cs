using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin {
	public static class StringExtensions {
		public static uint ToAccountNumber(this string accountString) {
			return AccountHelper.ParseAccount(accountString);
		}

		public static byte[] FromHexStringToByteArray(this string hex) {
			if (string.IsNullOrEmpty(hex))
				return new byte[0];

			var offset = 0;
			if (hex.StartsWith("0x"))
				offset = 2;

			var numberChars = (hex.Length - offset) / 2;

			var bytes = new byte[numberChars];
			using (var stringReader = new StringReader(hex)) {
				for (var i = 0; i < numberChars; i++) {
					if (i >= offset)
						bytes[i - offset] = Convert.ToByte(new string(new char[2] { (char)stringReader.Read(), (char)stringReader.Read() }), 16);
				}
			}
			return bytes;
		}
	}
}
