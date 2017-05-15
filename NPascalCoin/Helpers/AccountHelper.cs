using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin {
	public static class AccountHelper {

		public static string ToAccountString(uint accountNo) {
			return $"{accountNo}-{CalculateAccountChecksum(accountNo)}";
		}

		public static byte CalculateAccountChecksum(uint accountNo) {
			var overflowSafeAccountNo = (ulong) accountNo;
			return (byte)(overflowSafeAccountNo * 101 % 89 + 10);
		}

		public static uint MaxMaturedAccount(uint blockHeight) {
			return (uint)((long)blockHeight - 100).ClipTo(0, uint.MaxValue) * Constants.AccountsPerBlock;
		}

		public static bool IsMatured(uint account, uint blockHeight) {
			return account <= MaxMaturedAccount(blockHeight);
		}
	}
}
