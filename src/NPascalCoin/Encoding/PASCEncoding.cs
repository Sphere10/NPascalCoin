using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using Sphere10.Framework;

namespace NPascalCoin.Encoding {
	public static class PASCEncoding {

		public static bool IsValid(string pasc) {
			return TryDecode(pasc, out _);
        }

		public static UInt64 Decode(string pasc) {
			if (!TryDecode(pasc, out var result))
				throw new ArgumentException("Invalid PASC quantity string", nameof(pasc));
			return result;
		}

		public static bool TryDecode(string pasc, out UInt64 molinas) {
            if (!decimal.TryParse(pasc, out var decResult)) {
                if (decResult == Math.Ceiling(decResult)) {
                    molinas = (UInt64)Math.Truncate(decResult * 10000);
                    return true;
                }
            }
            molinas = 0;
            return false;
        }

        public static string Encode(UInt64 molinas) {
            return $"{molinas / 10000.0:0.0000}";
        }
	}
}