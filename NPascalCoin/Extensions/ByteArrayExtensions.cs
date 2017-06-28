using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin {
	public static class ByteArrayExtensions {
		public static string ToHexString(this byte[] byteArray, bool ommit_0x = false) {
			if (byteArray.Length == 0)
				return string.Empty;

			var hexBuilder = new StringBuilder(byteArray.Length * 2);
			if (!ommit_0x)
				hexBuilder.Append("0x");

			foreach (var @byte in byteArray)
				hexBuilder.AppendFormat("{0:x2}", @byte);

			return hexBuilder.ToString();
		}
	}
}
