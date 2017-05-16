using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPascalCoin.DTO;

namespace NPascalCoin {
	public class PascalCoinException : ApplicationException {
		public PascalCoinException(string error) : base(error)  {			
		}
	}
}
