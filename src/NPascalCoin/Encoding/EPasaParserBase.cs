﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NPascalCoin.Accounts;
using NPascalCoin.API.Classic.Objects;
using NPascalCoin.Common;
using Sphere10.Framework;

namespace NPascalCoin.Encoding {
	public abstract class EPasaParserBase : IEPasaParser {

		public abstract bool TryParse(string epasaText, out EPasa epasa, out EPasaErrorCode errorCode);

	}
}
