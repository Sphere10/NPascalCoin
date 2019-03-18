using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sphere10.Framework;

using  NPascalCoin.Common;

namespace NPascalCoin.Common.Text {
	public interface IEPasaParser {
		bool TryParse(string epasaText, out EPasa epasa, out EPasaErrorCode errorCode);
	}

}
