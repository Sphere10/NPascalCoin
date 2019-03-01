using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sphere10.Framework;

namespace NPascalCoin.Common {

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

		public static uint ParseAccount(string accountString) {
			uint account;
			if (!TryParseAccount(accountString, out account)) {
				throw new PascalCoinException($"Invalid account string '{accountString}'");
			}
			return account;
		}

		public static bool TryParseAccount(string accountString, out uint account) {
			account = 0;
			accountString = accountString.Trim();
			var splits = accountString.Split('-');
			if (splits.Length != 2) {
				return false;
			}

			if (!uint.TryParse(splits[0], out account)) {
				return false;
			}

			uint checksum;
			if (!uint.TryParse(splits[1], out checksum)) {
				return false;
			}

			return checksum == CalculateAccountChecksum(account);
		}

	}
}
