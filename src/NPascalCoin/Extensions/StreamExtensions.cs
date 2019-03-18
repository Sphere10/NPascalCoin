using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPascalCoin.Common;

namespace NPascalCoin {
	public static class StreamExtensions {
		public static bool HasMoreBytes(this Stream stream, int byteCount) {
			return stream.Length - byteCount >= 0;
		}

	}
}
