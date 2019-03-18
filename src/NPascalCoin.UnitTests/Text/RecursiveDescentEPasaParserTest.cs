using System;
using System.Collections.Generic;
using System.Text;
using NPascalCoin.Common.Text;
using NPascalCoin.UnitTests;

namespace NPascalCoin.UnitTests.Text {
	public class RecursiveDescentEPasaParserTest : EPasaTests {
		public override IEPasaParser NewInstance() {
			return new RecursiveDescentEPasaParser();
		}
	}
}
