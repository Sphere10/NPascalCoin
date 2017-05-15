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

namespace NPascalCoin.RPC {
	public class PascalCoinRPCException : ApplicationException {

		public PascalCoinRPCException(string error) : base(error)  {			
		}

		public PascalCoinRPCException(ErrorResultDTO result) : base($"{result.ErrorCode} - {result.Message}") {
			Error = result;
		}

		public ErrorResultDTO Error { get; }

	}
}
