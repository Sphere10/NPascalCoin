using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin {
	internal static class NumericExtensions {
		public static TimeSpan ClipTo(this TimeSpan value, TimeSpan min, TimeSpan max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}

		public static double ClipTo(this double value, double min, double max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}

		public static float ClipTo(this float value, float min, float max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}

		public static decimal ClipTo(this decimal value, decimal min, decimal max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}


		public static sbyte ClipTo(this sbyte value, sbyte min, sbyte max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}

		public static byte ClipTo(this byte value, byte min, byte max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}


		public static short ClipTo(this short value, short min, short max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}

		public static ushort ClipTo(this ushort value, ushort min, ushort max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}


		public static int ClipTo(this int value, int min, int max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}

		public static uint ClipTo(this uint value, uint min, uint max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}

		public static long ClipTo(this long value, long min, long max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}

		public static ulong ClipTo(this ulong value, ulong min, ulong max) {
			if (value < min) {
				return min;
			} else if (value > max) {
				return max;
			}
			return value;
		}

		public static void Repeat(this int times, Action action) {
			for (int i = 0; i < times; i++) {
				action();
			}
		}

	}
}
