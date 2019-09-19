using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NPascalCoin.Accounts;
using Sphere10.Framework;

using  NPascalCoin.Common;

namespace NPascalCoin.Encoding {
	public interface IEPasaParser {
		bool TryParse(string epasaText, out EPasa epasa, out EPasaErrorCode errorCode);
	}

}
