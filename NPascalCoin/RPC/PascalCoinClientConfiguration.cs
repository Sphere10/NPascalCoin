using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
	public class PascalCoinClientConfiguration {
		public PascalCoinNetwork Network { get; set; } // For Future
		public string Server { get; set; }
		public int Port { get; set; }
		public string User { get; set; } // For future
		public string Password { get; set; } // For future

		public static PascalCoinClientConfiguration DefaultMainNet => new PascalCoinClientConfiguration {
			Network = PascalCoinNetwork.MainNet,
			Server = "localhost",
			Port = Constants.DefaultMainNetRpcPort,
			User = null,
			Password = null
		};

		public static PascalCoinClientConfiguration DefaultTestNet => new PascalCoinClientConfiguration {
			Network = PascalCoinNetwork.Testnet,
			Server = "localhost",
			Port = Constants.DefaultTestNetRpcPort,
			User = null,
			Password = null
		};
	} 
}
