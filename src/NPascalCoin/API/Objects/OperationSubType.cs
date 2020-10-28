using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace

namespace NPascalCoin.API.Objects {

	public enum OperationSubType {
		TransactionSender = 11,		
		TransactionReceiver = 12,
		BuyTransactionBuyer = 13,
		BuyTransactionTarget = 14,
		BuyTransactionSeller = 15,
		ChangeKey = 21,
		Recover = 31,
		ListAccountForPublicSale = 41,
		ListAccountForPrivateSale = 42,
		DelistAccount = 51,
		BuyAccountBuyer = 61,
		BuyAccountTarget = 62,
		BuyAccountSeller = 63,
		ChangeKeySigned = 71,
		ChangeAccountInfo = 81,
	}
}
  