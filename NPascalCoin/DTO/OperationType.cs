using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPascalCoin.DTO {
	public enum OperationType {
		/// <summary>
		/// Blockchain reward
		/// </summary>
		BlockchainReward = 0,

		/// <summary>
		/// Transaction
		/// </summary>
		Transaction = 1,

		/// <summary>
		/// Change key
		/// </summary>
		ChangeKey = 2,

		/// <summary>
		/// Recover founds (lost keys)
		/// </summary>
		RecoverFunds = 3
	}

}
