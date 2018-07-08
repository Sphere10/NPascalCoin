using System;

namespace NPascalCoin {
	[Flags]
	public enum ChangeInfoType {
		PublicKey          = 1 << 0,
		AccountName        = 1 << 1,
		AccountType        = 1 << 2,
		ListForPublicSale  = 1 << 3,
		ListForPrivateSale = 1 << 4,
		DelistAccount      = 1 << 5
	}
}