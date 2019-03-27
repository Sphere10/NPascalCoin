using System.IO;

namespace NPascalCoin {
	public static class StreamExtensions {
		public static bool HasMoreBytes(this Stream stream, int byteCount) {
			return stream.Length - byteCount >= 0;
		}

	}
}
