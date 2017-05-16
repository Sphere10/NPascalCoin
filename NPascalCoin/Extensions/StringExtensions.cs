using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin {
	public static class StringExtensions {
		public static uint ToAccountNumber(this string accountString) {
			return AccountHelper.ParseAccount(accountString);
		}
	}
}
