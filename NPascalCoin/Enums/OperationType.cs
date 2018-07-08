using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace NPascalCoin {
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
		RecoverFunds = 3,

		/// <summary>
		/// List account for sale
		/// </summary>
		ListAccountForSale = 4,

		/// <summary>
		/// Delist account from sale
		/// </summary>
		DelistAccount = 5,

		/// <summary>
		/// Buy account on sale
		/// </summary>
		BuyAccount = 6,

		/// <summary>
		/// Change key with other key signer
		/// </summary>
		ChangeKeySigned = 7,

		/// <summary>
		/// Change account info (name, type)
		/// </summary>
		ChangeAccountInfo = 8,

		/// <summary>
		/// Multi-operation
		/// </summary>
		MultiOperation = 9
	}
}