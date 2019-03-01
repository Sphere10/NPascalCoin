using NPascalCoin.API.Classic.Objects;

namespace NPascalCoin.API.Classic {
	public class PascalCoinClientConfiguration {
		public PascalCoinNetwork Network { get; set; } // For Future
		public string Server { get; set; }
		public int Port { get; set; }
		public string User { get; set; } // For future
		public string Password { get; set; } // For future

		public static PascalCoinClientConfiguration DefaultMainNet => new PascalCoinClientConfiguration {
			Network = PascalCoinNetwork.MainNet,
			Server = "localhost",
			Port = Constants.DefaultMainNetClassicRpcPort,
			User = null,
			Password = null
		};

		public static PascalCoinClientConfiguration DefaultTestNet => new PascalCoinClientConfiguration {
			Network = PascalCoinNetwork.Testnet,
			Server = "localhost",
			Port = Constants.DefaultTestNetClassicRpcPort,
			User = null,
			Password = null
		};
	} 
}
